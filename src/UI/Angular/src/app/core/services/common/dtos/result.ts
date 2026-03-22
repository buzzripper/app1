import { ResultError } from './result-error';

export interface Result<T = void> {
	isSuccess: boolean;
	error?: ResultError;
	data?: T;
}
