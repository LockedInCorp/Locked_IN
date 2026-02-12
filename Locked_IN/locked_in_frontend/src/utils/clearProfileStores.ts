import { useProfileStore } from "@/stores/profileStore"
import { useGameProfilesStore } from "@/stores/gameProfilesStore"

export function clearProfileStores(): void {
    useProfileStore.getState().resetProfile()
    useGameProfilesStore.getState().resetGameProfilesUI()
}
