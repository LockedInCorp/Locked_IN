import { useEffect, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import { API_BASE_URL } from '@/api/apiClient';
import type { TeamJoinStatusDto } from '@/api/types';

const BASE_URL = API_BASE_URL.replace('/api', '');

type JoinRequestStatusHandler = (data: TeamJoinStatusDto) => void;

export function useTeamJoinHub(onJoinRequestStatus?: JoinRequestStatusHandler) {
    const connectionRef = useRef<signalR.HubConnection | null>(null);
    const handlerRef = useRef(onJoinRequestStatus);
    const isSettingUpRef = useRef(false);

    useEffect(() => {
        handlerRef.current = onJoinRequestStatus;
    }, [onJoinRequestStatus]);

    useEffect(() => {
        if (connectionRef.current || isSettingUpRef.current) {
            return;
        }

        isSettingUpRef.current = true;
        let isMounted = true;

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`${BASE_URL}/teamjoinhub`)
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveJoinRequestStatus", (data: TeamJoinStatusDto) => {
            if (handlerRef.current) {
                handlerRef.current(data);
            }
        });

        connection.start()
            .then(() => {
                if (isMounted) {
                    console.log("Connected to SignalR hub");
                    isSettingUpRef.current = false;
                }
            })
            .catch((err) => {
                if (isMounted) {
                    console.error("SignalR connection error:", err);
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
                        .then(() => console.log("Disconnected from SignalR hub"))
                        .catch((err) => {
                            if (err.name !== 'AbortError') {
                                console.error("SignalR disconnection error:", err);
                            }
                        });
                }
            }
        };
    }, []);

    return {
        connection: connectionRef.current,
    };
}
