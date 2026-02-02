export interface UserProfileDto {
  id: number;
  email: string;
  username: string;
  avatar?: File | string;
  avatarURL?: string;
  token?: string;
  availability?: Record<string, string[]>;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  avatar?: File | null;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface UserProfileResponse {
    id: number
    email: string
    username: string
    avatarURL?: string
    availability?: Record<string, string[]>
}

export interface UpdateProfileRequest {
    username: string
    email: string
    avatar?: File | null;
}

export interface UpdateAvailabilityRequest {
    availability: Record<string, string[]>
}

export interface GameProfile {
    id: number
    userId: number
    gameId: number
    gameName: string
    isFavorite: boolean
    rank?: string
}

export interface GameProfileResponse {
    success: boolean
    message: string
    data?: GameProfile | GameProfile[]
}

export interface GameDto {
    id: number
    name: string
}

export interface TagsResponse {
    success: boolean
    message: string
    data?: {
        games: Array<{
            id: number
            name: string
        }>
        experienceTags: Array<{
            id: number
            experiencelevel: string
        }>
        preferenceTags: Array<{
            id: number
            name: string
        }>
        gameExperiences: Array<{
            id: number
            experience: string
        }>
        gameplayPreferences: Array<{
            id: number
            preference: string
        }>
    }
}

export interface UserChatDto {
    id: number;
    chatName?: string;
    lastMessageUsername?: string;
    lastMessageContent?: string;
    lastMessageTime?: string | Date;
    unreadMessageCount: number;
    chatIconUrl?: string;
    creationTimestamp: string | Date;
}

export interface ChatMessageDto {
    id: number;
    senderUsername: string;
    content: string;
    timestamp: string | Date;
    isCurrentUser: boolean;
}

export interface GroupDetailsDto {
    id: number;
    name: string;
    description?: string;
    avatarUrl?: string;
    memberCount: number;
    tags: string[];
    experience: string;
    communicationService?: string;
    games: string[];
    members: GroupMemberDto[];
    requests: GroupRequestDto[];
}

export interface GroupMemberDto {
    id: number;
    username: string;
    avatarUrl?: string;
}

export interface GroupRequestDto {
    id: number;
    username: string;
    avatarUrl?: string;
}

export interface CreateGroupRequest {
    name: string;
    game: string;
    maxMembers: number;
    isBlitz: boolean;
    autoAccept: boolean;
    previewImage?: File | null;
    tags: string[];
    experience: string;
    minCompetitiveScore?: number;
    communicationService?: string;
    description?: string;
}