"use client"

import { Button } from "@/components/ui/button"
import GeneralSection from "@/custom_components/groupCreation/GeneralSection"
import FinderSettingsSection from "@/custom_components/groupCreation/FinderSettingsSection"
import { useGroupCreationStore } from "@/stores/groupCreationStore"

export default function GroupCreation() {
    const {
        selectedTags,
        blitzRoom,
        autoAccept,
        experience,
        groupSize,
        toggleTag,
        setBlitzRoom,
        setAutoAccept,
        setExperience,
        setGroupSize
    } = useGroupCreationStore()

    return (
        <main className="flex flex-col h-full w-full overflow-hidden">
            {/* Scrollable Content */}
            <div className="flex-1 overflow-y-auto min-h-0">
                <div className="px-6 pt-12 pb-12">
                    <h2 className="mb-8 text-center text-3xl font-bold text-foreground">Group Creation</h2>

                    <div className="flex flex-col items-center">
                        <div className="grid gap-6 lg:grid-cols-2 max-w-5xl">
                            <GeneralSection
                                blitzRoom={blitzRoom}
                                setBlitzRoom={setBlitzRoom}
                                autoAccept={autoAccept}
                                setAutoAccept={setAutoAccept}
                                groupSize={groupSize}
                                setGroupSize={setGroupSize}
                            />
                            <FinderSettingsSection
                                selectedTags={selectedTags}
                                toggleTag={toggleTag}
                                experience={experience}
                                setExperience={setExperience}
                            />
                        </div>

                        <div className="mt-8 w-full max-w-5xl flex justify-end">
                            <Button size="lg" className="bg-primary px-12 text-base font-semibold text-primary-foreground hover:bg-primary/90 cursor-pointer">
                                Create
                            </Button>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    )
}
