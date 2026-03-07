import { StatusLevel } from "./status-level";

export interface HealthStatus {
    status: StatusLevel;
    message: string;
    timestamp: string;
}
