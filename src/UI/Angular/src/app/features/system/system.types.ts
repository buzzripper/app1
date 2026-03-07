export interface SystemTileStatus {
    module: 'auth' | 'app' | 'portal' | 'adagent';
    title: string;
    pingStatus: string;
    healthStatus: string;
    pingMessage?: string;
    healthMessage?: string;
}
