export interface PingResult {
    module: string;
    service: string;
    timestamp: string;
}

export interface HealthStatus {
    isHealthy: boolean;
    message: string;
    timestamp: string;
}

export interface AuthHealthStatus extends HealthStatus {}
