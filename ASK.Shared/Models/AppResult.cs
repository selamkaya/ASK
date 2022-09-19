using ASK.Shared.Interfaces;

namespace ASK.Shared.Models;

public class AppResult : IAppResult
{
	public List<string> Messages { get; set; } = new List<string>();

	public bool Succeeded { get; set; }

	public static IAppResult Fail()
	{
		return new AppResult { Succeeded = false };
	}

	public static IAppResult Fail(string message)
	{
		return new AppResult { Succeeded = false, Messages = new List<string> { message } };
	}

	public static IAppResult Fail(List<string> messages)
	{
		return new AppResult { Succeeded = false, Messages = messages };
	}

	public static Task<IAppResult> FailAsync()
	{
		return Task.FromResult(Fail());
	}

	public static Task<IAppResult> FailAsync(string message)
	{
		return Task.FromResult(Fail(message));
	}

	public static Task<IAppResult> FailAsync(List<string> messages)
	{
		return Task.FromResult(Fail(messages));
	}

	public static IAppResult Success()
	{
		return new AppResult { Succeeded = true };
	}

	public static IAppResult Success(string message)
	{
		return new AppResult { Succeeded = true, Messages = new List<string> { message } };
	}

	public static Task<IAppResult> SuccessAsync()
	{
		return Task.FromResult(Success());
	}

	public static Task<IAppResult> SuccessAsync(string message)
	{
		return Task.FromResult(Success(message));
	}
}

public class AppResult<TResult> : AppResult, IAppResult<TResult>
{
	public TResult Data { get; set; }

	public new static AppResult<TResult> Fail()
	{
		return new AppResult<TResult> { Succeeded = false };
	}

	public new static AppResult<TResult> Fail(string message)
	{
		return new AppResult<TResult> { Succeeded = false, Messages = new List<string> { message } };
	}

	public new static AppResult<TResult> Fail(List<string> messages)
	{
		return new AppResult<TResult> { Succeeded = false, Messages = messages };
	}

	public new static Task<AppResult<TResult>> FailAsync()
	{
		return Task.FromResult(Fail());
	}

	public new static Task<AppResult<TResult>> FailAsync(string message)
	{
		return Task.FromResult(Fail(message));
	}

	public new static Task<AppResult<TResult>> FailAsync(List<string> messages)
	{
		return Task.FromResult(Fail(messages));
	}

	public new static AppResult<TResult> Success()
	{
		return new AppResult<TResult> { Succeeded = true };
	}

	public new static AppResult<TResult> Success(string message)
	{
		return new AppResult<TResult> { Succeeded = true, Messages = new List<string> { message } };
	}

	public static AppResult<TResult> Success(TResult data)
	{
		return new AppResult<TResult> { Succeeded = true, Data = data };
	}

	public static AppResult<TResult> Success(TResult data, string message)
	{
		return new AppResult<TResult> { Succeeded = true, Data = data, Messages = new List<string> { message } };
	}

	public static AppResult<TResult> Success(TResult data, List<string> messages)
	{
		return new AppResult<TResult> { Succeeded = true, Data = data, Messages = messages };
	}

	public new static Task<AppResult<TResult>> SuccessAsync()
	{
		return Task.FromResult(Success());
	}

	public new static Task<AppResult<TResult>> SuccessAsync(string message)
	{
		return Task.FromResult(Success(message));
	}

	public static Task<AppResult<TResult>> SuccessAsync(TResult data)
	{
		return Task.FromResult(Success(data));
	}

	public static Task<AppResult<TResult>> SuccessAsync(TResult data, string message)
	{
		return Task.FromResult(Success(data, message));
	}
}