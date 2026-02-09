"use client"

import { Upload, X } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Switch } from "@/components/ui/switch"
import { Textarea } from "@/components/ui/textarea"
import { useRef, useState, useEffect } from "react"
import { searchGamesByName } from "@/api/api.ts"
import type {GameDto} from "@/api/types.ts"

type GeneralSectionProps = {
    groupName?: string
    setGroupName?: (v: string) => void
    gameId?: number | null
    gameName?: string
    setGame?: (id: number | null, name: string) => void
    isPrivate: boolean
    setIsPrivate: (v: boolean) => void
    autoAccept: boolean
    setAutoAccept: (v: boolean) => void
    groupSize: string
    setGroupSize?: (v: string) => void
    previewImage?: File | null
    setPreviewImage?: (file: File | null) => void
    description?: string
    setDescription?: (v: string) => void
    disableGroupSize?: boolean
    disableIsPrivate?: boolean
}

export default function GeneralSection({
                                           groupName = "",
                                           setGroupName,
                                           gameId = null,
                                           gameName = "",
                                           setGame,
                                           isPrivate,
                                           setIsPrivate,
                                           autoAccept,
                                           setAutoAccept,
                                           groupSize,
                                           setGroupSize,
                                           previewImage,
                                           setPreviewImage,
                                           description = "",
                                           setDescription,
                                           disableGroupSize = false,
                                           disableIsPrivate = false,
                                       }: GeneralSectionProps) {
    const fileInputRef = useRef<HTMLInputElement>(null)
    const [error, setError] = useState<string | null>(null)

    const [searchTerm, setSearchTerm] = useState(gameName)
    const [searchResults, setSearchResults] = useState<GameDto[]>([])
    const [isDropdownOpen, setIsDropdownOpen] = useState(false)
    const [isLoading, setIsLoading] = useState(false)
    const dropdownRef = useRef<HTMLDivElement>(null)

    useEffect(() => {
        setSearchTerm(gameName)
    }, [gameName])

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
                setIsDropdownOpen(false)
            }
        }
        document.addEventListener("mousedown", handleClickOutside)
        return () => document.removeEventListener("mousedown", handleClickOutside)
    }, [])

    useEffect(() => {
        if (!searchTerm || searchTerm === gameName || !isDropdownOpen) {
            setSearchResults([])
            return
        }

        const timer = setTimeout(async () => {
            setIsLoading(true)
            try {
                const results = await searchGamesByName(searchTerm)
                setSearchResults(results)
            } catch (err) {
                console.error("Failed to search games:", err)
            } finally {
                setIsLoading(false)
            }
        }, 300)

        return () => clearTimeout(timer)
    }, [searchTerm, gameName, isDropdownOpen])

    const handleSelectGame = (selectedGame: GameDto) => {
        setGame?.(selectedGame.id, selectedGame.name)
        setSearchTerm(selectedGame.name)
        setIsDropdownOpen(false)
    }

    const handleClearGame = () => {
        setGame?.(null, "")
        setSearchTerm("")
        setIsDropdownOpen(true)
    }

    useEffect(() => {
        if (groupSize === "") return

        const size = parseInt(groupSize)
        if (isNaN(size) || size < 2 || size > 20) {
            setError("Group size must be between 2 and 20")
        } else {
            setError(null)
        }
    }, [])

    const handleGroupSizeChange = (v: string) => {
        if (v !== "" && !/^\d+$/.test(v)) return

        if (setGroupSize) {
            setGroupSize(v)
        }

        if (v === "") {
            setError("Group size is required")
            return
        }

        const size = parseInt(v)
        if (isNaN(size) || size < 2 || size > 20) {
            setError("Group size must be between 2 and 20")
        } else {
            setError(null)
        }
    }

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0] || null
        if (setPreviewImage) {
            setPreviewImage(file)
        }
    }

    return (
        <div className="rounded-lg border border-border bg-card p-6">
            <h3 className="mb-6 text-xl font-semibold text-foreground">General</h3>

            <div className="space-y-6">
                {/* Group Name */}
                <div className="space-y-2">
                    <Label htmlFor="group-name" className="text-sm text-muted-foreground">
                        Group Name
                    </Label>
                    <Input
                        id="group-name"
                        placeholder="Lock IN! group"
                        value={groupName}
                        onChange={(e) => setGroupName?.(e.target.value)}
                        className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                    />
                </div>

                {/* Game */}
                <div className="space-y-2">
                    <Label htmlFor="game" className="text-sm text-muted-foreground">
                        Game
                    </Label>
                    <div className="relative h-10" ref={dropdownRef}>
                        {gameId ? (
                            <div className="flex h-10 w-full items-center justify-between rounded-md border border-border bg-muted/50 px-3 py-2">
                                <span className="text-sm font-medium text-foreground truncate">{gameName}</span>
                                <Button
                                    type="button"
                                    variant="ghost"
                                    size="icon"
                                    onClick={handleClearGame}
                                    className="h-6 w-6 text-muted-foreground hover:text-foreground cursor-pointer"
                                >
                                    <X className="h-4 w-4" />
                                </Button>
                            </div>
                        ) : (
                            <>
                                <Input
                                    id="game"
                                    placeholder="Search for a game..."
                                    value={searchTerm}
                                    onChange={(e) => {
                                        setSearchTerm(e.target.value)
                                        setIsDropdownOpen(true)
                                    }}
                                    onFocus={() => setIsDropdownOpen(true)}
                                    className="h-10 border-border bg-card text-foreground placeholder:text-muted-foreground"
                                    autoComplete="off"
                                />
                                {isDropdownOpen && (searchTerm.length > 0) && (
                                    <div className="absolute z-50 mt-1 max-h-60 w-full overflow-auto rounded-md border border-border bg-card shadow-lg">
                                        {isLoading ? (
                                            <div className="px-4 py-2 text-sm text-muted-foreground">Searching...</div>
                                        ) : searchResults.length > 0 ? (
                                            <ul className="py-1">
                                                {searchResults.map((g) => (
                                                    <li
                                                        key={g.id}
                                                        onClick={() => handleSelectGame(g)}
                                                        className="cursor-pointer px-4 py-2 text-sm text-foreground hover:bg-muted"
                                                    >
                                                        {g.name}
                                                    </li>
                                                ))}
                                            </ul>
                                        ) : (
                                            <div className="px-4 py-2 text-sm text-muted-foreground">No games found</div>
                                        )}
                                    </div>
                                )}
                            </>
                        )}
                    </div>
                </div>

                {/* Group Size */}
                <div className="space-y-2">
                    <Label htmlFor="group-size" className="text-sm text-muted-foreground">
                        Group Size
                    </Label>
                    <Input
                        id="group-size"
                        type="text"
                        inputMode="numeric"
                        placeholder="Min 2, Max 20"
                        value={groupSize}
                        onChange={(e) => handleGroupSizeChange(e.target.value)}
                        disabled={disableGroupSize}
                        className={`border-border bg-card text-foreground placeholder:text-muted-foreground ${error ? 'border-destructive' : ''} ${disableGroupSize ? 'opacity-50 cursor-not-allowed' : ''}`}
                    />
                    {error && (
                        <p className="text-xs text-destructive">{error}</p>
                    )}
                </div>

                {/* Private Group */}
                <div className={`flex items-start justify-between gap-4 ${disableIsPrivate ? 'opacity-50' : ''}`}>
                    <div className="space-y-1">
                        <Label className="text-sm font-medium text-foreground">Private Group</Label>
                        <p className="text-xs text-muted-foreground">Group will be deleted after the last player leaves</p>
                    </div>
                    <Switch
                        checked={isPrivate}
                        onCheckedChange={disableIsPrivate ? undefined : setIsPrivate}
                        disabled={disableIsPrivate}
                        className="data-[state=checked]:bg-primary"
                    />
                </div>

                {/* Auto-accept */}
                <div className="flex items-start justify-between gap-4">
                    <div className="space-y-1">
                        <Label className="text-sm font-medium text-foreground">Auto-accept</Label>
                        <p className="text-xs text-muted-foreground">You will not have to manually accept players join requests</p>
                    </div>
                    <Switch
                        checked={autoAccept}
                        onCheckedChange={setAutoAccept}
                        className="data-[state=checked]:bg-primary"
                    />
                </div>

                {/* Group Preview Image */}
                <div className="space-y-2">
                    <Label className="text-sm text-muted-foreground">Group Preview Image (optional)</Label>
                    <input
                        ref={fileInputRef}
                        type="file"
                        accept="image/*"
                        onChange={handleFileChange}
                        className="hidden"
                    />
                    <Button
                        type="button"
                        variant="outline"
                        onClick={() => fileInputRef.current?.click()}
                        className="border-border bg-card text-muted-foreground hover:bg-muted hover:text-foreground cursor-pointer"
                    >
                        <Upload className="mr-2 size-4" />
                        {previewImage ? previewImage.name : "Choose file"}
                    </Button>
                </div>

                {/* Description */}
                <div className="space-y-2">
                    <Label htmlFor="description" className="text-sm text-muted-foreground">
                        Description (optional)
                    </Label>
                    <Textarea
                        id="description"
                        placeholder="Type your description here"
                        value={description}
                        onChange={(e) => setDescription?.(e.target.value)}
                        className="min-h-32 resize-none border-border bg-card text-foreground placeholder:text-muted-foreground"
                    />
                </div>
            </div>
        </div>
    )
}
