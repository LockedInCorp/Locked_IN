"use client"

import { Button } from "@/lib/components/ui/button"
import GeneralSection from "@/components/groupCreation/GeneralSection"
import FinderSettingsSection from "@/components/groupCreation/FinderSettingsSection"
import { useGroupEditStore } from "@/stores/groupEditStore"
import { useGroupEdit } from "@/hooks/group/useGroupEdit"
import { useParams } from "react-router-dom"

export default function GroupEdit() {
    const { id } = useParams<{ id: string }>()
    const { isLoading, isSaving, error, handleSave } = useGroupEdit(id)

    const {
        groupName,
        gameId,
        gameName,
        groupSize,
        isPrivate,
        autoAccept,
        previewImage,
        selectedTags,
        experience,
        competitiveScore,
        communicationService,
        communicationLink,
        description,
        setGroupName,
        setGame,
        setIsPrivate,
        setAutoAccept,
        setPreviewImage,
        toggleTag,
        setExperience,
        setCompetitiveScore,
        setCommunicationService,
        setCommunicationLink,
        setDescription
    } = useGroupEditStore()

    return (
        <main className="flex flex-col h-full w-full overflow-hidden">
            <div className="flex-1 overflow-y-auto min-h-0">
                <div className="px-6 pt-12 pb-12">
                    <h2 className="mb-8 text-center text-3xl font-bold text-foreground">Group Edit</h2>

                    {error && (
                        <div className="mb-6 p-4 rounded-md bg-destructive/10 text-destructive text-sm">
                            {error}
                        </div>
                    )}

                    {isLoading ? (
                        <div className="flex justify-center py-12">Loading...</div>
                    ) : (
                    <div className="flex flex-col items-center">
                        <div className="grid gap-6 lg:grid-cols-2 max-w-5xl">
                            <GeneralSection
                                groupName={groupName}
                                setGroupName={setGroupName}
                                gameId={gameId}
                                gameName={gameName}
                                setGame={setGame}
                                isPrivate={isPrivate}
                                setIsPrivate={setIsPrivate}
                                autoAccept={autoAccept}
                                setAutoAccept={setAutoAccept}
                                groupSize={groupSize}
                                previewImage={previewImage}
                                setPreviewImage={setPreviewImage}
                                description={description}
                                setDescription={setDescription}
                                disableGroupSize={true}
                                disableIsPrivate={true}
                            />
                            <FinderSettingsSection
                                selectedTags={selectedTags}
                                toggleTag={toggleTag}
                                experience={experience}
                                setExperience={setExperience}
                                competitiveScore={competitiveScore}
                                setCompetitiveScore={setCompetitiveScore}
                                communicationService={communicationService}
                                setCommunicationService={setCommunicationService}
                                communicationLink={communicationLink}
                                setCommunicationLink={setCommunicationLink}
                            />
                        </div>

                        <div className="mt-8 w-full max-w-5xl flex justify-end">
                            <Button 
                                size="lg" 
                                onClick={handleSave}
                                disabled={isSaving}
                                className="bg-primary px-12 text-base font-semibold text-primary-foreground hover:bg-primary/90 cursor-pointer"
                            >
                                {isSaving ? "Saving..." : "Save"}
                            </Button>
                        </div>
                    </div>
                    )}
                </div>
            </div>
        </main>
    )
}

