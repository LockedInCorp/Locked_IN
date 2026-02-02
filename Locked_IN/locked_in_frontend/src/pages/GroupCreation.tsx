"use client"

import { Button } from "@/components/ui/button"
import GeneralSection from "@/custom_components/groupCreation/GeneralSection"
import FinderSettingsSection from "@/custom_components/groupCreation/FinderSettingsSection"
import { useGroupCreationStore } from "@/stores/groupCreationStore"
import createTeam from "@/api/api"
import { useState } from "react"
import { useNavigate } from "react-router-dom"

export default function GroupCreation() {
    const navigate = useNavigate()
    const [previewImage, setPreviewImage] = useState<File | null>(null)
    const [isSubmitting, setIsSubmitting] = useState(false)

    const {
        selectedTags,
        isPrivate,
        autoAccept,
        experience,
        competitiveScore,
        groupSize,
        gameId,
        gameName,
        groupName,
        description,
        communicationService,
        communicationLink,
        toggleTag,
        setIsPrivate,
        setAutoAccept,
        setExperience,
        setCompetitiveScore,
        setGroupSize,
        setGame,
        setGroupName,
        setDescription,
        setCommunicationService,
        setCommunicationLink,
        resetForm
    } = useGroupCreationStore()

    const handleCreate = async () => {
        if (!gameId || !groupName || !groupSize || !experience) {
            alert("Please fill in all required fields (Group Name, Game, Group Size, and Experience Level)")
            return
        }

        setIsSubmitting(true)
        try {
            await createTeam({
                name: groupName,
                gameId: gameId,
                maxMembers: parseInt(groupSize),
                isPrivate: isPrivate,
                autoAccept: autoAccept,
                experience: experience,
                tags: selectedTags,
                minCompetitiveScore: parseInt(competitiveScore) || 0,
                communicationService: communicationService,
                communicationServiceLink: communicationLink,
                description: description,
                previewImage: previewImage
            })
            alert("Team created successfully!")
            resetForm()
            setPreviewImage(null)
            navigate('/discover') // Or wherever appropriate
        } catch (error: any) {
            alert(error.message || "Failed to create team")
        } finally {
            setIsSubmitting(false)
        }
    }

    return (
        <main className="flex flex-col h-full w-full overflow-hidden">
            <div className="flex-1 overflow-y-auto min-h-0">
                <div className="px-6 pt-12 pb-12">
                    <h2 className="mb-8 text-center text-3xl font-bold text-foreground">Group Creation</h2>

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
                                setGroupSize={setGroupSize}
                                description={description}
                                setDescription={setDescription}
                                previewImage={previewImage}
                                setPreviewImage={setPreviewImage}
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
                                className="bg-primary px-12 text-base font-semibold text-primary-foreground hover:bg-primary/90 cursor-pointer"
                                onClick={handleCreate}
                                disabled={isSubmitting}
                            >
                                {isSubmitting ? "Creating..." : "Create"}
                            </Button>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    )
}
