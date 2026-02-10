import { useQuery } from "@tanstack/react-query"
import { getUserChats } from "@/api/api"

export function useUserChats() {
    return useQuery({
        queryKey: ["userChats"],
        queryFn: getUserChats,
    })
}
