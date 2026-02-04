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

export interface GameSuggestion {
    id: string
    label: string
}



export interface ExperienceTag {
    id: number
    name: string
}

export interface PreferenceTag {
    id: number
    name: string
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
    communicationService?: number;
    communicationServiceLink?: string;
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
    gameId: number;
    maxMembers: number;
    isPrivate: boolean;
    autoAccept: boolean;
    previewImage?: File | null;
    tags: number[];
    experience: number;
    minCompetitiveScore?: number;
    communicationService?: number;
    communicationServiceLink?: string;
    description?: string;
}

export interface CommunicationService {
    id: number;
    name: string;
}

export interface TeamSearchRequest {
    gameIds: number[];
    preferenceTagIds: number[];
    searchTerm: string;
    page: number;
    pageSize: number;
    sortBy: string;
    showPendingRequests: boolean;
}

export interface TeamSearchResult {
    id: number;
    name: string;
    minCompScore: number;
    maxPlayerCount: number;
    description: string;
    gameId: number;
    gameName: string;
    isPrivate: boolean;
    experienceTagId: number;
    experienceLevel: string;
    currentMemberCount: number;
    members: TeamMemberSearchResult[];
    preferenceTags: string[];
    creationTimestamp: string;
    iconUrl: string;
    searchRank: number;
    teamLeaderUsername: string;
}

export interface TeamMemberSearchResult {
    id: number;
    isLeader: boolean;
    joinTimestamp: string;
    teamId: number;
    userId: number;
    memberStatusId: number;
    memberStatusName: string;
    user: {
        id: number;
        email: string;
        nickname: string;
        availability: string;
    };
}

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
}