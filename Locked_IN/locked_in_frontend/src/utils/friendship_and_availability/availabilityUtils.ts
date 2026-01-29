export type Availability = Record<string, string[]>

export const isCellAvailable = (
    day: string,
    hour: number,
    availability: Availability
): boolean => {
    const dayAvailability = availability[day] || []
    if (dayAvailability.length === 0) return false

    for (let i = 0; i < dayAvailability.length; i += 2) {
        const startHour = parseInt(dayAvailability[i]?.split(':')[0] || '0')
        const endHour = parseInt(dayAvailability[i + 1]?.split(':')[0] || '0')
        
        if (hour >= startHour && hour < endHour) {
            return true
        }
    }
    return false
}

export const toggleHour = (
    day: string,
    hour: number,
    currentAvailability: Availability
): Availability => {
    const newAvailability = { ...currentAvailability }
    const dayHours = newAvailability[day] || []
    const hourStr = hour.toString().padStart(2, '0') + ':00'
    const nextHourStr = (hour + 1).toString().padStart(2, '0') + ':00'
    
    let hourInRange = false
    let rangeIndex = -1
    
    for (let i = 0; i < dayHours.length; i += 2) {
        const startHour = parseInt(dayHours[i]?.split(':')[0] || '0')
        const endHour = parseInt(dayHours[i + 1]?.split(':')[0] || '0')
        
        if (hour >= startHour && hour < endHour) {
            hourInRange = true
            rangeIndex = i
            break
        }
    }
    
    let newDayHours: string[] = []
    
    if (hourInRange && rangeIndex !== -1) {
        for (let i = 0; i < dayHours.length; i += 2) {
            const startHour = parseInt(dayHours[i]?.split(':')[0] || '0')
            const endHour = parseInt(dayHours[i + 1]?.split(':')[0] || '0')
            
            if (i === rangeIndex) {
                if (startHour === hour && endHour === hour + 1) {
                    continue
                } else if (startHour === hour) {
                    newDayHours.push(nextHourStr, dayHours[i + 1])
                } else if (endHour === hour + 1) {
                    newDayHours.push(dayHours[i], hourStr)
                } else {
                    newDayHours.push(dayHours[i], hourStr)
                    newDayHours.push(nextHourStr, dayHours[i + 1])
                }
            } else {
                newDayHours.push(dayHours[i], dayHours[i + 1])
            }
        }
    } else {
        let inserted = false
        for (let i = 0; i < dayHours.length; i += 2) {
            const startHour = parseInt(dayHours[i]?.split(':')[0] || '0')
            
            if (hour === startHour - 1 && !inserted) {
                newDayHours.push(hourStr, dayHours[i + 1])
                inserted = true
            } else if (hour < startHour && !inserted) {
                newDayHours.push(hourStr, nextHourStr)
                inserted = true
            }
            
            if (hour === parseInt(dayHours[i + 1]?.split(':')[0] || '0') && !inserted) {
                newDayHours.push(dayHours[i], nextHourStr)
                inserted = true
            } else {
                newDayHours.push(dayHours[i], dayHours[i + 1])
            }
        }
        
        if (!inserted) {
            newDayHours.push(hourStr, nextHourStr)
        }
        
        newDayHours.sort((a, b) => {
            const hourA = parseInt(a.split(':')[0] || '0')
            const hourB = parseInt(b.split(':')[0] || '0')
            return hourA - hourB
        })
    }
    
    if (newDayHours.length > 0) {
        newAvailability[day] = newDayHours
    } else {
        delete newAvailability[day]
    }
    
    return newAvailability
}
