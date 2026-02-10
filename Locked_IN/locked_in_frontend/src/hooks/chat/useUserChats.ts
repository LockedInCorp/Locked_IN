import { useQuery } from "@tanstack/react-query"
import { getUserChats } from "@/api/api"

export function useUserChats(searchTerm?: string) {
    return useQuery({
        queryKey: ["userChats", searchTerm],
        queryFn: () => getUserChats(searchTerm),
    })
}
