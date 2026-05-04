import { ResultError } from './result-error';

export class ResultException extends Error {
	readonly error: ResultError;

	constructor(error: ResultError) {
		super(error.message);
		this.name = 'ResultException';
		this.error = error;
	}
}
