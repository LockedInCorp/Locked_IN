"use client"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import GeneralSection from "@/custom_components/groupCreation/GeneralSection"
import FinderSettingsSection from "@/custom_components/groupCreation/FinderSettingsSection"

export default function GroupCreation() {
    const [selectedTags, setSelectedTags] = useState<string[]>([])
    const [blitzRoom, setBlitzRoom] = useState(false)
    const [autoAccept, setAutoAccept] = useState(false)
    const [experience, setExperience] = useState("beginner")

    const toggleTag = (tag: string) => {
        setSelectedTags(prev => (prev.includes(tag) ? prev.filter(t => t !== tag) : [...prev, tag]))
    }

    return (
        <div className="min-h-screen">
            <main className="mx-auto max-w-7xl px-6 py-12">
                <h2 className="mb-8 text-center text-3xl font-bold text-white">Group Creation</h2>

                <div className="grid gap-6 lg:grid-cols-2">
                    <GeneralSection
                        blitzRoom={blitzRoom}
                        setBlitzRoom={setBlitzRoom}
                        autoAccept={autoAccept}
                        setAutoAccept={setAutoAccept}
                    />
                    <FinderSettingsSection
                        selectedTags={selectedTags}
                        toggleTag={toggleTag}
                        experience={experience}
                        setExperience={setExperience}
                    />
                </div>

                <div className="mt-8 flex justify-end">
                    <Button size="lg" className="bg-primary px-12 text-base font-semibold text-white hover:bg-primary/90">
                        Create
                    </Button>
                </div>
            </main>
        </div>
    )
}
