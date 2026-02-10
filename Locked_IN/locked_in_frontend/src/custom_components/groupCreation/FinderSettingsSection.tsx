"use client"

import { useEffect, useState } from "react"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { getPreferenceTags, getExperienceTags, getCommunicationServices } from "@/api/api"
import type {PreferenceTag, ExperienceTag, CommunicationService} from "@/api/types"

type FinderSettingsProps = {
    selectedTags: number[]
    toggleTag: (tagId: number) => void
    experience: number
    setExperience: (v: number) => void
    competitiveScore?: string
    setCompetitiveScore?: (v: string) => void
    communicationService?: number | null
    setCommunicationService?: (v?: number) => void
    communicationLink?: string
    setCommunicationLink?: (v: string) => void
}

export default function FinderSettingsSection({
                                                  selectedTags,
                                                  toggleTag,
                                                  experience,
                                                  setExperience,
                                                  competitiveScore = "0",
                                                  setCompetitiveScore,
                                                  communicationService,
                                                  setCommunicationService,
                                                  communicationLink,
                                                  setCommunicationLink,
                                              }: FinderSettingsProps) {
    const [availableTags, setAvailableTags] = useState<PreferenceTag[]>([])
    const [availableExperienceLevels, setAvailableExperienceLevels] = useState<ExperienceTag[]>([])
    const [availableCommunicationServices, setAvailableCommunicationServices] = useState<CommunicationService[]>([])

    useEffect(() => {
        const fetchTags = async () => {
            try {
                const [tags, experiences, communicationServices] = await Promise.all([
                    getPreferenceTags(),
                    getExperienceTags(),
                    getCommunicationServices()
                ])
                setAvailableTags(tags)
                setAvailableExperienceLevels(experiences)
                setAvailableCommunicationServices(communicationServices)
            } catch (error) {
                console.error("Failed to fetch tags:", error)
            }
        }
        fetchTags()
    }, [])

    return (
        <div className="rounded-lg border border-border bg-card p-6">
            <h3 className="mb-6 text-xl font-semibold text-foreground">Finder Settings</h3>

            <div className="space-y-6">
                {/* Game Tags */}
                <div className="space-y-3">
                    <Label className="text-sm text-muted-foreground">Game Tags</Label>
                    <div className="flex flex-wrap gap-2">
                        {availableTags.map(tag => (
                            <button
                                key={tag.id}
                                onClick={() => toggleTag(tag.id)}
                                className={`flex items-center rounded-md px-4 py-2 text-sm font-medium transition-all cursor-pointer ${
                                    selectedTags.includes(tag.id)
                                        ? "bg-primary text-primary-foreground border-2 border-primary shadow-sm"
                                        : "bg-card text-muted-foreground border-2 border-transparent hover:bg-muted hover:text-foreground hover:border-border"
                                }`}
                            >
                                {tag.name}
                            </button>
                        ))}
                    </div>
                </div>

                {/* Group Experience */}
                <div className="space-y-3">
                    <Label className="text-sm text-muted-foreground">Group Experience</Label>
                    <RadioGroup value={experience.toString()} onValueChange={(v) => setExperience(parseInt(v))}>
                        {availableExperienceLevels.map(level => (
                            <div key={level.id} className="flex items-center gap-2">
                                <RadioGroupItem value={level.id.toString()} id={level.id.toString()} className="border-border" />
                                <Label htmlFor={level.id.toString()} className="cursor-pointer text-sm text-foreground capitalize">
                                    {level.name}
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
                        value={competitiveScore}
                        onChange={(e) => setCompetitiveScore?.(e.target.value)}
                        className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                    />
                </div>

                {/* Communication Service */}
                <div className="space-y-2">
                    <Label htmlFor="communication-service" className="text-sm text-muted-foreground">
                        Communication Service
                    </Label>
                    <Select 
                        value={communicationService?.toString() || "none"} 
                        onValueChange={(value) => setCommunicationService?.(value === "none" ? undefined : parseInt(value))}
                    >
                        <SelectTrigger id="communication-service" className="w-full border-border bg-card text-foreground">
                            <SelectValue placeholder="Select service" />
                        </SelectTrigger>
                        <SelectContent className="border-border bg-card">
                            <SelectItem value="none" className="text-foreground">None</SelectItem>
                            {availableCommunicationServices.map((service) => (
                                <SelectItem key={service.id} value={service.id.toString()} className="text-foreground">
                                    {service.name}
                                </SelectItem>
                            ))}
                        </SelectContent>
                    </Select>
                </div>

                {/* Communication Service Join Link */}
                {!!communicationService && (
                    <div className="space-y-2">
                        <Label htmlFor="communication-link" className="text-sm text-muted-foreground">
                            Communication Join Link
                        </Label>
                        <Input
                            id="communication-link"
                            type="text"
                            placeholder="https://..."
                            value={communicationLink}
                            onChange={(e) => setCommunicationLink?.(e.target.value)}
                            className="border-border bg-card text-foreground placeholder:text-muted-foreground"
                        />
                    </div>
                )}
            </div>
        </div>
    )
}
