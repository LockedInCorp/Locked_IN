"use client"

import { Upload } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Switch } from "@/components/ui/switch"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { useRef } from "react"

type GeneralSectionProps = {
    groupName?: string
    setGroupName?: (v: string) => void
    game?: string
    setGame?: (v: string) => void
    blitzRoom: boolean
    setBlitzRoom: (v: boolean) => void
    autoAccept: boolean
    setAutoAccept: (v: boolean) => void
    groupSize: string
    setGroupSize?: (v: string) => void
    previewImage?: File | null
    setPreviewImage?: (file: File | null) => void
    disableGroupSize?: boolean
    disableBlitzRoom?: boolean
}

export default function GeneralSection({
                                           groupName = "",
                                           setGroupName,
                                           game = "",
                                           setGame,
                                           blitzRoom,
                                           setBlitzRoom,
                                           autoAccept,
                                           setAutoAccept,
                                           groupSize,
                                           setGroupSize,
                                           previewImage,
                                           setPreviewImage,
                                           disableGroupSize = false,
                                           disableBlitzRoom = false,
                                       }: GeneralSectionProps) {
    const fileInputRef = useRef<HTMLInputElement>(null)

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
                    <Input
                        id="game"
                        placeholder="ex. Terraria, Overwatch 2"
                        value={game}
                        onChange={(e) => setGame?.(e.target.value)}
                        className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                    />
                </div>

                {/* Group Size */}
                <div className="space-y-2">
                    <Label htmlFor="group-size" className="text-sm text-muted-foreground">
                        Group Size
                    </Label>
                    <Select 
                        value={groupSize} 
                        onValueChange={setGroupSize || (() => {})}
                        disabled={disableGroupSize}
                    >
                        <SelectTrigger 
                            id="group-size" 
                            className={`w-full border-border bg-card text-foreground ${disableGroupSize ? 'opacity-50 cursor-not-allowed' : ''}`}
                        >
                            <SelectValue placeholder="Select group size" />
                        </SelectTrigger>
                        <SelectContent className="border-border bg-card">
                            {Array.from({ length: 19 }, (_, i) => i + 2).map(size => (
                                <SelectItem key={size} value={size.toString()} className="text-foreground">
                                    {size}
                                </SelectItem>
                            ))}
                        </SelectContent>
                    </Select>
                </div>

                {/* Blitz-room */}
                <div className={`flex items-start justify-between gap-4 ${disableBlitzRoom ? 'opacity-50' : ''}`}>
                    <div className="space-y-1">
                        <Label className="text-sm font-medium text-foreground">Blitz-room</Label>
                        <p className="text-xs text-muted-foreground">Group will be deleted after the last player leaves</p>
                    </div>
                    <Switch
                        checked={blitzRoom}
                        onCheckedChange={disableBlitzRoom ? undefined : setBlitzRoom}
                        disabled={disableBlitzRoom}
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
            </div>
        </div>
    )
}
