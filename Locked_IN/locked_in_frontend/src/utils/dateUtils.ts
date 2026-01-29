export const formatDate = (dateString: string): string => {
    const date = new Date(dateString)
    return date.toLocaleDateString('en-US', { month: '2-digit', day: '2-digit', year: 'numeric' })
}

export const formatTimeAgo = (dateString: string): string => {
    const date = new Date(dateString)
    const now = new Date()
    const diffMs = now.getTime() - date.getTime()
    const diffMins = Math.floor(diffMs / 60000)
    
    if (diffMins < 60) {
        return `${diffMins} min ago`
    } else if (diffMins < 1440) {
        const hours = Math.floor(diffMins / 60)
        return `${hours} hour${hours > 1 ? 's' : ''} ago`
    } else {
        const days = Math.floor(diffMins / 1440)
        return `${days} day${days > 1 ? 's' : ''} ago`
    }
}
