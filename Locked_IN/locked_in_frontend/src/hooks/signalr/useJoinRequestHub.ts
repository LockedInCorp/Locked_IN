import { useEffect, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import { API_BASE_URL } from '@/api/apiClient';
import type { TeamJoinResponceDto } from '@/api/types';

const BASE_URL = API_BASE_URL.replace('/api', '');

type NewJoinRequestHandler = (data: TeamJoinResponceDto) => void;
type CanceledJoinRequestHandler = (userId: number, teamId: number) => void;

export function useJoinRequestHub(
    onNewJoinRequest?: NewJoinRequestHandler,
    onCanceledJoinRequest?: CanceledJoinRequestHandler,
    enabled: boolean = true
) {
    const connectionRef = useRef<signalR.HubConnection | null>(null);
    const newHandlerRef = useRef(onNewJoinRequest);
    const canceledHandlerRef = useRef(onCanceledJoinRequest);
    const isSettingUpRef = useRef(false);

    useEffect(() => {
        newHandlerRef.current = onNewJoinRequest;
    }, [onNewJoinRequest]);

    useEffect(() => {
        canceledHandlerRef.current = onCanceledJoinRequest;
    }, [onCanceledJoinRequest]);

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
            .withUrl(`${BASE_URL}/teamrequesthub`)
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveNewJoinRequest", (data: TeamJoinResponceDto) => {
            if (newHandlerRef.current) {
                newHandlerRef.current(data);
            }
        });

        connection.on("ReceiveCanceledJoinRequest", (userId: number, teamId: number) => {
            if (canceledHandlerRef.current) {
                canceledHandlerRef.current(userId, teamId);
            }
        });

        connection.start()
            .then(() => {
                if (isMounted) {
                    console.log("Connected to JoinRequest SignalR hub");
                    isSettingUpRef.current = false;
                }
            })
            .catch((err) => {
                if (isMounted) {
                    console.error("JoinRequest SignalR connection error:", err);
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
                        .then(() => console.log("Disconnected from JoinRequest SignalR hub"))
                        .catch((err) => {
                            if (err.name !== 'AbortError') {
                                console.error("JoinRequest SignalR disconnection error:", err);
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
