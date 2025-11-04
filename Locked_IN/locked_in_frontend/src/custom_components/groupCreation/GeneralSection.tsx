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
        <div className="rounded-lg border border-[#3a3d42] bg-[#1a1d21] p-6">
            <h3 className="mb-6 text-xl font-semibold text-white">General</h3>

            <div className="space-y-6">
                {/* Group Name */}
                <div className="space-y-2">
                    <Label htmlFor="group-name" className="text-sm text-gray-300">
                        Group Name
                    </Label>
                    <Input
                        id="group-name"
                        placeholder="Lock IN! group"
                        className="border-[#3a3d42] bg-[#2b2d31] text-white placeholder:text-gray-500"
                    />
                </div>

                {/* Game */}
                <div className="space-y-2">
                    <Label htmlFor="game" className="text-sm text-gray-300">
                        Game
                    </Label>
                    <Input
                        id="game"
                        placeholder="ex. Terraria, Overwatch 2"
                        className="border-[#3a3d42] bg-[#2b2d31] text-white placeholder:text-gray-500"
                    />
                </div>

                {/* Group Size */}
                <div className="space-y-2">
                    <Label htmlFor="group-size" className="text-sm text-gray-300">
                        Group Size
                    </Label>
                    <Select defaultValue="value">
                        <SelectTrigger id="group-size" className="w-full border-[#3a3d42] bg-[#2b2d31] text-white">
                            <SelectValue placeholder="Select size" />
                        </SelectTrigger>
                        <SelectContent className="border-[#3a3d42] bg-[#2b2d31]">
                            <SelectItem value="2-4" className="text-white">2–4 players</SelectItem>
                            <SelectItem value="5-10" className="text-white">5–10 players</SelectItem>
                            <SelectItem value="10+" className="text-white">10+ players</SelectItem>
                        </SelectContent>
                    </Select>
                </div>

                {/* Blitz-room */}
                <div className="flex items-start justify-between gap-4">
                    <div className="space-y-1">
                        <Label className="text-sm font-medium text-white">Blitz-room</Label>
                        <p className="text-xs text-gray-400">Group will be deleted after the last player leaves</p>
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
                        <Label className="text-sm font-medium text-white">Auto-accept</Label>
                        <p className="text-xs text-gray-400">You will not have to manually accept players join requests</p>
                    </div>
                    <Switch
                        checked={autoAccept}
                        onCheckedChange={setAutoAccept}
                        className="data-[state=checked]:bg-primary"
                    />
                </div>

                {/* Group Preview Image */}
                <div className="space-y-2">
                    <Label className="text-sm text-gray-300">Group Preview Image (optional)</Label>
                    <Button
                        variant="outline"
                        className="border-[#3a3d42] bg-[#2b2d31] text-gray-300 hover:bg-[#3a3d42] hover:text-white"
                    >
                        <Upload className="mr-2 size-4" />
                        Choose file
                    </Button>
                </div>
            </div>
        </div>
    )
}
