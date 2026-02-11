"use client"

import { useEffect } from "react"
import { useParams, useNavigate } from "react-router-dom"
import { Button } from "@/components/ui/button"
import { ArrowLeft, X } from "lucide-react"
import ProfileHeader from "@/custom_components/profile/ProfileHeader"
import ProfileFields from "@/custom_components/profile/ProfileFields"
import { useFriendProfile } from "@/hooks/friendship/useFriendProfile"
import { useAuthStore } from "@/stores/authStore"

export default function FriendProfile() {
    const { id } = useParams<{ id: string }>()
    const navigate = useNavigate()
    const { user } = useAuthStore()
    const friendId = id ? parseInt(id) : 0

    useEffect(() => {
        if (user?.id && friendId > 0 && friendId.toString() === user.id) {
            navigate("/profile", { replace: true })
        }
    }, [user?.id, friendId, navigate])

    if (!id || isNaN(friendId) || friendId <= 0) {
        return (
            <div className="relative w-full min-h-full overflow-y-auto bg-background">
                <div className="relative z-10 min-h-full flex items-center justify-center px-6 py-12">
                    <div className="text-center">
                        <p className="text-destructive mb-4">Invalid user ID</p>
                        <Button onClick={() => navigate(-1)} variant="outline">
                            <ArrowLeft className="mr-2 h-4 w-4" />
                            Back
                        </Button>
                    </div>
                </div>
            </div>
        )
    }

    const {
        isLoading,
        isActionLoading,
        error,
        friendshipStatus,
        isOutgoingRequest,
        profileData,
        handleAddFriend,
        handleCancelRequest,
        handleDeleteFriend
    } = useFriendProfile(friendId)

    const handleBack = () => {
        navigate(-1)
    }

    const getButtonContent = () => {
        if (friendshipStatus === "Pending" && isOutgoingRequest) {
            return (
                <Button
                    variant="outline"
                    onClick={handleCancelRequest}
                    disabled={isActionLoading}
                    className="px-6 py-2 text-base font-semibold border-border bg-muted text-muted-foreground hover:bg-muted/80 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
                >
                    <X className="mr-2 h-4 w-4" />
                    Cancel
                </Button>
            )
        } else if (friendshipStatus === "Accepted") {
            return (
                <Button
                    onClick={handleDeleteFriend}
                    disabled={isActionLoading}
                    className="px-6 py-2 text-base font-semibold bg-destructive text-destructive-foreground hover:bg-destructive/90 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
                >
                    {isActionLoading ? "Deleting..." : "Delete"}
                </Button>
            )
        } else {
            return (
                <Button
                    onClick={handleAddFriend}
                    disabled={isActionLoading}
                    className="px-6 py-2 text-base font-semibold bg-orange-500 text-white hover:bg-orange-600 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
                >
                    {isActionLoading ? "Sending..." : "Add friend"}
                </Button>
            )
        }
    }

    if (isLoading) {
        return (
            <div className="relative w-full min-h-full overflow-y-auto bg-background">
                <div className="relative z-10 min-h-full flex items-center justify-center px-6 py-12">
                    <div className="text-center">
                        <p className="text-muted-foreground">Loading profile...</p>
                    </div>
                </div>
            </div>
        )
    }

    if (!profileData) {
        return (
            <div className="relative w-full min-h-full overflow-y-auto bg-background">
                <div className="relative z-10 min-h-full flex items-center justify-center px-6 py-12">
                    <div className="text-center">
                        <p className="text-destructive">Failed to load profile</p>
                        <Button onClick={handleBack} variant="outline" className="mt-4">
                            <ArrowLeft className="mr-2 h-4 w-4" />
                            Back
                        </Button>
                    </div>
                </div>
            </div>
        )
    }

    return (
        <div className="relative w-full min-h-full overflow-y-auto bg-background">
            <div className="relative z-10 min-h-full flex items-center justify-center px-6 py-12">
                <div className="w-full max-w-3xl">
                    <div className="rounded-lg border border-border bg-card p-8 shadow-lg">
                        {error && (
                            <div className="mb-4 p-3 rounded-md bg-destructive/10 border border-destructive text-destructive text-sm">
                                {error}
                            </div>
                        )}

                        <ProfileHeader 
                            avatarUrl={profileData.avatarUrl}
                            avatarFallback={profileData.avatarFallback}
                            isEditing={false}
                            title={`${profileData.nickname}'s Profile`}
                        />

                        <ProfileFields
                            nickname={profileData.nickname}
                            gameProfiles={profileData.gameProfiles}
                        />

                        <div className="mt-8 flex justify-between items-center">
                            <Button 
                                variant="outline"
                                onClick={handleBack}
                                className="px-6 py-2 text-base font-semibold border-border hover:bg-muted cursor-pointer"
                            >
                                <ArrowLeft className="mr-2 h-4 w-4" />
                                Back
                            </Button>
                            
                            <div className="flex gap-3">
                                {getButtonContent()}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
