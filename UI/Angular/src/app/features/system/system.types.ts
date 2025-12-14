export interface SystemTileStatus {
    module: 'auth' | 'app' | 'portal';
    title: string;
    pingStatus: 'unknown' | 'success' | 'error';
    healthStatus: 'unknown' | 'success' | 'error';
    pingMessage?: string;
    healthMessage?: string;
}
