import { firstValueFrom, Observable } from 'rxjs';
import { Result } from './dtos/result';
import { ResultException } from './dtos/result-exception';

function throwIfFailure(result: Result<unknown>): void {
	if (!result.isSuccess) {
		throw new ResultException(result.error!);
	}
}

export async function unwrapResult<T>(source: Observable<Result<T>>): Promise<T> {
	const result = await firstValueFrom(source);
	throwIfFailure(result);
	return result.data as T;
}

export async function unwrapVoidResult(source: Observable<Result<void>>): Promise<void> {
	const result = await firstValueFrom(source);
	throwIfFailure(result);
}
