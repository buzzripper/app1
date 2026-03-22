import { ResultErrorKind } from './result-error-kind';

export interface ResultError {
	kind: ResultErrorKind;
	message: string;
	fieldErrors?: Record<string, string[]>;
}
