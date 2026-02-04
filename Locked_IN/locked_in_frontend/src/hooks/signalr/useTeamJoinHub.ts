import { useEffect, useRef, useCallback } from 'react';
import * as signalR from '@microsoft/signalr';
import { API_BASE_URL } from '@/api/apiClient';
import type { TeamJoinStatusDto } from '@/api/types';

type JoinRequestStatusHandler = (data: TeamJoinStatusDto) => void;

export function useTeamJoinHub(onJoinRequestStatus?: JoinRequestStatusHandler) {
    const connectionRef = useRef<signalR.HubConnection | null>(null);
    const handlerRef = useRef(onJoinRequestStatus);

    useEffect(() => {
        handlerRef.current = onJoinRequestStatus;
    }, [onJoinRequestStatus]);

    const setupSignalRConnection = useCallback(() => {
        if (connectionRef.current) {
            return;
        }

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`${API_BASE_URL}/teamjoinhub`)
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveJoinRequestStatus", (data: TeamJoinStatusDto) => {
            if (handlerRef.current) {
                handlerRef.current(data);
            }
        });

        connection.start()
            .then(() => console.log("Connected to SignalR hub"))
            .catch((err) => console.error("SignalR connection error:", err));

        connectionRef.current = connection;
    }, []);

    useEffect(() => {
        setupSignalRConnection();

        return () => {
            if (connectionRef.current) {
                connectionRef.current.stop()
                    .then(() => console.log("Disconnected from SignalR hub"))
                    .catch((err) => console.error("SignalR disconnection error:", err));
                connectionRef.current = null;
            }
        };
    }, [setupSignalRConnection]);

    return {
        connection: connectionRef.current,
    };
}
