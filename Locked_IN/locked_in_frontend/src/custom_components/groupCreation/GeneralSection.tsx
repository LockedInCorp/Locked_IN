"use client"

import { Upload } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Switch } from "@/components/ui/switch"

type GeneralSectionProps = {
    blitzRoom: boolean
    setBlitzRoom: (v: boolean) => void
    autoAccept: boolean
    setAutoAccept: (v: boolean) => void
}

export default function GeneralSection({
                                           blitzRoom,
                                           setBlitzRoom,
                                           autoAccept,
                                           setAutoAccept,
                                       }: GeneralSectionProps) {
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
                        className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                    />
                </div>

                {/* Group Size */}
                <div className="space-y-2">
                    <Label htmlFor="group-size" className="text-sm text-muted-foreground">
                        Group Size
                    </Label>
                    <Select defaultValue="value">
                        <SelectTrigger id="group-size" className="w-full border-border bg-card text-foreground">
                            <SelectValue placeholder="Select size" />
                        </SelectTrigger>
                        <SelectContent className="border-border bg-card">
                            <SelectItem value="2-4" className="text-foreground">2–4 players</SelectItem>
                            <SelectItem value="5-10" className="text-foreground">5–10 players</SelectItem>
                            <SelectItem value="10+" className="text-foreground">10+ players</SelectItem>
                        </SelectContent>
                    </Select>
                </div>

                {/* Blitz-room */}
                <div className="flex items-start justify-between gap-4">
                    <div className="space-y-1">
                        <Label className="text-sm font-medium text-foreground">Blitz-room</Label>
                        <p className="text-xs text-muted-foreground">Group will be deleted after the last player leaves</p>
                    </div>
                    <Switch
                        checked={blitzRoom}
                        onCheckedChange={setBlitzRoom}
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
                    <Button
                        variant="outline"
                        className="border-border bg-card text-muted-foreground hover:bg-muted hover:text-foreground cursor-pointer"
                    >
                        <Upload className="mr-2 size-4" />
                        Choose file
                    </Button>
                </div>
            </div>
        </div>
    )
}
