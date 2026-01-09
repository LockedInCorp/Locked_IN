"use client"

import { useEffect } from "react"
import { useParams, useNavigate } from "react-router-dom"
import { Button } from "@/components/ui/button"
import GeneralSection from "@/custom_components/groupCreation/GeneralSection"
import FinderSettingsSection from "@/custom_components/groupCreation/FinderSettingsSection"
import { useGroupEditStore } from "@/stores/groupEditStore"

export default function GroupEdit() {
    const { id } = useParams<{ id: string }>()
    const navigate = useNavigate()
    
    const {
        groupName,
        game,
        groupSize,
        blitzRoom,
        autoAccept,
        previewImage,
        selectedTags,
        experience,
        competitiveScore,
        communicationService,
        description,
        setGroupName,
        setGame,
        setBlitzRoom,
        setAutoAccept,
        setPreviewImage,
        toggleTag,
        setExperience,
        setCompetitiveScore,
        setCommunicationService,
        setDescription,
        loadTeamData,
        resetForm
    } = useGroupEditStore()

    useEffect(() => {
        if (id) {
            // TODO: Fetch team data from API
        }

        return () => {
            resetForm()
        }
    }, [id, loadTeamData, resetForm])

    const handleSave = async () => {
        // TODO: Implement API call to update team
        
        console.log("Saving team:", {
            groupName,
            game,
            groupSize,
            blitzRoom,
            autoAccept,
            selectedTags,
            experience,
            competitiveScore,
            communicationService,
            description
        })
        
        navigate("/my-groups")
    }

    return (
        <main className="flex flex-col h-full w-full overflow-hidden">
            {/* Scrollable Content */}
            <div className="flex-1 overflow-y-auto min-h-0">
                <div className="px-6 pt-12 pb-12">
                    <h2 className="mb-8 text-center text-3xl font-bold text-foreground">Group Edit</h2>

                    <div className="flex flex-col items-center">
                        <div className="grid gap-6 lg:grid-cols-2 max-w-5xl">
                            <GeneralSection
                                groupName={groupName}
                                setGroupName={setGroupName}
                                game={game}
                                setGame={setGame}
                                blitzRoom={blitzRoom}
                                setBlitzRoom={setBlitzRoom}
                                autoAccept={autoAccept}
                                setAutoAccept={setAutoAccept}
                                groupSize={groupSize}
                                previewImage={previewImage}
                                setPreviewImage={setPreviewImage}
                                disableGroupSize={true}
                                disableBlitzRoom={true}
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
                                description={description}
                                setDescription={setDescription}
                            />
                        </div>

                        <div className="mt-8 w-full max-w-5xl flex justify-end">
                            <Button 
                                size="lg" 
                                onClick={handleSave}
                                className="bg-primary px-12 text-base font-semibold text-primary-foreground hover:bg-primary/90 cursor-pointer"
                            >
                                Save
                            </Button>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    )
}

