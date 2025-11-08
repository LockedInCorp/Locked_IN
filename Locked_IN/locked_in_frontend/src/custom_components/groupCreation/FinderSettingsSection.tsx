"use client"

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
        <div className="rounded-lg border border-border bg-card p-6">
            <h3 className="mb-6 text-xl font-semibold text-foreground">Finder Settings</h3>

            <div className="space-y-6">
                {/* Game Tags */}
                <div className="space-y-3">
                    <Label className="text-sm text-muted-foreground">Game Tags</Label>
                    <div className="flex flex-wrap gap-2">
                        {gameTags.map(tag => (
                            <button
                                key={tag}
                                onClick={() => toggleTag(tag)}
                                className={`flex items-center rounded-md px-4 py-2 text-sm font-medium transition-all ${
                                    selectedTags.includes(tag)
                                        ? "bg-primary text-primary-foreground border-2 border-primary shadow-sm"
                                        : "bg-card text-muted-foreground border-2 border-transparent hover:bg-muted hover:text-foreground hover:border-border"
                                }`}
                            >
                                {tag}
                            </button>
                        ))}
                    </div>
                </div>

                {/* Group Experience */}
                <div className="space-y-3">
                    <Label className="text-sm text-muted-foreground">Group Experience</Label>
                    <RadioGroup value={experience} onValueChange={setExperience}>
                        {["beginner", "experienced", "professional"].map(level => (
                            <div key={level} className="flex items-center gap-2">
                                <RadioGroupItem value={level} id={level} className="border-border" />
                                <Label htmlFor={level} className="cursor-pointer text-sm text-foreground capitalize">
                                    {level}
                                </Label>
                            </div>
                        ))}
                    </RadioGroup>
                </div>

                {/* Minimal Competitive Score */}
                <div className="space-y-2">
                    <Label htmlFor="competitive-score" className="text-sm text-muted-foreground">
                        Minimal Competitive score (optional)
                    </Label>
                    <Input
                        id="competitive-score"
                        type="number"
                        placeholder="0"
                        defaultValue="0"
                        className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                    />
                </div>

                {/* Communication Service */}
                <div className="space-y-2">
                    <Label htmlFor="communication-service" className="text-sm text-muted-foreground">
                        Communication Service
                    </Label>
                    <Select defaultValue="discord">
                        <SelectTrigger id="communication-service" className="w-full border-border bg-card text-foreground">
                            <SelectValue placeholder="Select service" />
                        </SelectTrigger>
                        <SelectContent className="border-border bg-card">
                            <SelectItem value="discord" className="text-foreground">Discord</SelectItem>
                            <SelectItem value="teamspeak" className="text-foreground">TeamSpeak</SelectItem>
                            <SelectItem value="mumble" className="text-foreground">Mumble</SelectItem>
                        </SelectContent>
                    </Select>
                </div>

                {/* Description */}
                <div className="space-y-2">
                    <Label htmlFor="description" className="text-sm text-muted-foreground">
                        Description (optional)
                    </Label>
                    <Textarea
                        id="description"
                        placeholder="Type your description here"
                        className="min-h-32 resize-none border-border bg-card text-foreground placeholder:text-muted-foreground"
                    />
                </div>
            </div>
        </div>
    )
}
