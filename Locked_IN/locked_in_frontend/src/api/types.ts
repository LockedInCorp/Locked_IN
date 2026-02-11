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


export const ChatType = {
    Direct: "Direct",
    Team: "Team"
} as const satisfies Record<string, string>

export type ChatType = typeof ChatType[keyof typeof ChatType]

export interface UserChatDto {
    id: number;
    chatName?: string;
    lastMessageUsername?: string;
    lastMessageContent?: string;
    lastMessageTime?: string | Date;
    unreadMessageCount: number;
    chatIconUrl?: string;
    creationTimestamp: string | Date;
    chatType?: ChatType;
}

export interface ChatMessageDto {
    id: number;
    senderUsername: string;
    content: string;
    timestamp: string | Date;
    isCurrentUser: boolean;
}

export interface GetMessageDto {
    id: number;
    content: string;
    sentAt: string | Date;
    editedAt?: string | Date;
    isDeleted: boolean;
    attachmentUrl?: string;
    senderId: number;
    senderUsername: string;
    senderAvatarUrl?: string;
    chatId: number;
}

export interface GetChatDetails {
    id: number;
    chatName?: string;
    chatType: string;
    teamId?: number;
    participantCount: number;
    chatIconUrl?: string;
    messageDtos: GetMessageDto[];
}

export interface GroupDetailsDto {
    id: number;
    name: string;
    description?: string;
    iconUrl?: string;
    currentMemberCount: number;
    preferenceTags: PreferenceTag[];
    experienceLevel: ExperienceTag;
    communicationService?: CommunicationService;
    communicationServiceLink?: string;
    game: GameDto;
    members: GroupMemberDto[];
    leader: GroupMemberDto;

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

export interface JoinRequestDto {
    teamId: number;
    userId: number;
    username: string;
    requestTimestamp: string;
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

export const TeamMemberStatus = {
    STATUS_LEADER: 1,
    STATUS_MEMBER: 2,
    STATUS_PENDING: 3,
    STATUS_REJECTED: 4,
    STATUS_NONE: 5
} as const satisfies Record<string, number>

export type TeamMemberStatus = 1 | 2 | 3 | 4 | 5

export interface TeamSearchResult {
    id: number;
    name: string;
    minCompScore: number;
    maxPlayerCount: number;
    description: string;
    game: GameDto;
    isPrivate: boolean;
    autoAccept: boolean;
    experienceLevel: ExperienceTag
    currentMemberCount: number;
    members: TeamMemberSearchResult[];
    preferenceTags: PreferenceTag[];
    creationTimestamp: string;
    iconUrl: string;
    searchRank: number;
    teamLeaderUsername: string;
    teamMemberStatus: TeamMemberStatus;
    chatId: number;
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

export interface TeamJoinStatusDto {
    teamId: number;
    teamName: string;
    status: number;
}

export interface TeamJoinResponceDto {
    teamId: number;
    userId: number;
    username: string;
    avatarUrl?: string;
    status: number;
    requestTimestamp: string;
}


export interface SendMessageRequest {
    chatId: number;
    content: string;
    attachmentFile?: File | null;
}