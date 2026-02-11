import { useEffect, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import { API_BASE_URL } from '@/api/apiClient';
import type { GetMessageDto, GroupMemberDto } from '@/api/types';

const BASE_URL = API_BASE_URL.replace('/api', '');

type UserJoinedHandler = (user: GroupMemberDto) => void;
type UserLeftHandler = (userId: number) => void;
type ReceiveMessageHandler = (message: GetMessageDto) => void;
type MessageEditedHandler = (message: GetMessageDto) => void;
type MessageDeletedHandler = (messageId: number) => void;

export interface ChatHubHandlers {
    onUserJoined?: UserJoinedHandler;
    onUserLeft?: UserLeftHandler;
    onReceiveMessage?: ReceiveMessageHandler;
    onMessageEdited?: MessageEditedHandler;
    onMessageDeleted?: MessageDeletedHandler;
}

export function useChatHub(
    handlers: ChatHubHandlers,
    enabled: boolean = true
) {
    const connectionRef = useRef<signalR.HubConnection | null>(null);
    const handlersRef = useRef(handlers);
    const isSettingUpRef = useRef(false);

    useEffect(() => {
        handlersRef.current = handlers;
    }, [handlers]);

    useEffect(() => {
        if (!enabled) {
            if (connectionRef.current) {
                connectionRef.current.stop();
                connectionRef.current = null;
            }
            return;
        }

        if (connectionRef.current || isSettingUpRef.current) {
            return;
        }

        isSettingUpRef.current = true;
        let isMounted = true;

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`${BASE_URL}/chathub`)
            .withAutomaticReconnect()
            .build();

        connection.on("UserJoined", (user: GroupMemberDto) => {
            handlersRef.current.onUserJoined?.(user);
        });

        connection.on("UserLeft", (userId: number) => {
            handlersRef.current.onUserLeft?.(userId);
        });

        connection.on("ReceiveMessage", (message: GetMessageDto) => {
            handlersRef.current.onReceiveMessage?.(message);
        });

        connection.on("MessageEdited", (message: GetMessageDto) => {
            handlersRef.current.onMessageEdited?.(message);
        });

        connection.on("MessageDeleted", (messageId: number) => {
            handlersRef.current.onMessageDeleted?.(messageId);
        });

        connection.start()
            .then(() => {
                if (isMounted) {
                    console.log("Connected to Chat SignalR hub");
                    isSettingUpRef.current = false;
                }
            })
            .catch((err) => {
                if (isMounted) {
                    console.error("Chat SignalR connection error:", err);
                    isSettingUpRef.current = false;
                    connectionRef.current = null;
                }
            });

        if (isMounted) {
            connectionRef.current = connection;
        }

        return () => {
            isMounted = false;
            isSettingUpRef.current = false;
            
            if (connectionRef.current) {
                const conn = connectionRef.current;
                connectionRef.current = null;
                
                if (conn.state !== signalR.HubConnectionState.Disconnected && 
                    conn.state !== signalR.HubConnectionState.Disconnecting) {
                    conn.stop()
                        .then(() => console.log("Disconnected from Chat SignalR hub"))
                        .catch((err) => {
                            if (err.name !== 'AbortError') {
                                console.error("Chat SignalR disconnection error:", err);
                            }
                        });
                }
            }
        };
    }, [enabled]);

    return {
        connection: connectionRef.current,
    };
}
