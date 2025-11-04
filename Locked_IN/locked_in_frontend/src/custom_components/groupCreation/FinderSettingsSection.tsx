"use client"

import { Check } from "lucide-react"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"

const gameTags = ["Chill", "Competitive", "Role-play", "Strategic", "Hardcore"]

type FinderSettingsProps = {
    selectedTags: string[]
    toggleTag: (tag: string) => void
    experience: string
    setExperience: (v: string) => void
}

export default function FinderSettingsSection({
                                                  selectedTags,
                                                  toggleTag,
                                                  experience,
                                                  setExperience,
                                              }: FinderSettingsProps) {
    return (
        <div className="rounded-lg border border-[#3a3d42] bg-[#1a1d21] p-6">
            <h3 className="mb-6 text-xl font-semibold text-white">Finder Settings</h3>

            <div className="space-y-6">
                {/* Game Tags */}
                <div className="space-y-3">
                    <Label className="text-sm text-gray-300">Game Tags</Label>
                    <div className="flex flex-wrap gap-2">
                        {gameTags.map(tag => (
                            <button
                                key={tag}
                                onClick={() => toggleTag(tag)}
                                className={`flex items-center gap-2 rounded-md px-4 py-2 text-sm font-medium transition-colors ${
                                    selectedTags.includes(tag)
                                        ? "bg-[#3a3d42] text-white"
                                        : "bg-[#2b2d31] text-gray-400 hover:bg-[#3a3d42] hover:text-white"
                                }`}
                            >
                                {selectedTags.includes(tag) && <Check className="size-4" />}
                                {tag}
                            </button>
                        ))}
                    </div>
                </div>

                {/* Group Experience */}
                <div className="space-y-3">
                    <Label className="text-sm text-gray-300">Group Experience</Label>
                    <RadioGroup value={experience} onValueChange={setExperience}>
                        {["beginner", "experienced", "professional"].map(level => (
                            <div key={level} className="flex items-center gap-2">
                                <RadioGroupItem value={level} id={level} className="border-[#3a3d42]" />
                                <Label htmlFor={level} className="cursor-pointer text-sm text-white capitalize">
                                    {level}
                                </Label>
                            </div>
                        ))}
                    </RadioGroup>
                </div>

                {/* Minimal Competitive Score */}
                <div className="space-y-2">
                    <Label htmlFor="competitive-score" className="text-sm text-gray-300">
                        Minimal Competitive score (optional)
                    </Label>
                    <Input
                        id="competitive-score"
                        type="number"
                        placeholder="0"
                        defaultValue="0"
                        className="border-[#3a3d42] bg-[#2b2d31] text-white placeholder:text-gray-500"
                    />
                </div>

                {/* Communication Service */}
                <div className="space-y-2">
                    <Label htmlFor="communication-service" className="text-sm text-gray-300">
                        Communication Service
                    </Label>
                    <Select defaultValue="discord">
                        <SelectTrigger id="communication-service" className="w-full border-[#3a3d42] bg-[#2b2d31] text-white">
                            <SelectValue placeholder="Select service" />
                        </SelectTrigger>
                        <SelectContent className="border-[#3a3d42] bg-[#2b2d31]">
                            <SelectItem value="discord" className="text-white">Discord</SelectItem>
                            <SelectItem value="teamspeak" className="text-white">TeamSpeak</SelectItem>
                            <SelectItem value="mumble" className="text-white">Mumble</SelectItem>
                        </SelectContent>
                    </Select>
                </div>

                {/* Description */}
                <div className="space-y-2">
                    <Label htmlFor="description" className="text-sm text-gray-300">
                        Description (optional)
                    </Label>
                    <Textarea
                        id="description"
                        placeholder="Type your description here"
                        className="min-h-32 resize-none border-[#3a3d42] bg-[#2b2d31] text-white placeholder:text-gray-500"
                    />
                </div>
            </div>
        </div>
    )
}
