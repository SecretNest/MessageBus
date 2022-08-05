namespace SecretNest.MessageBus
{
    // ReSharper disable TypeParameterCanBeVariant
    
    /// <summary>
    /// Delegate of a subscriber without return value.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <param name="argument">Argument.</param>
    public delegate void Subscriber<TParameter>(TParameter argument);

    /// <summary>
    /// Delegate of a subscriber with return value.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    /// <param name="argument">Argument.</param>
    /// <returns>Return value.</returns>
    public delegate TReturn Subscriber<TParameter, TReturn>(TParameter argument);

    /// <summary>
    /// Delegate of a subscriber with <see cref="MessageInstance"/> as an argument without return value.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <param name="argument">Argument.</param>
    /// <param name="messageInstance">The instance information of this executing.</param>
    public delegate void SubscriberWithMessageInstance<TParameter>(TParameter argument, MessageInstance messageInstance);

    /// <summary>
    /// Delegate of a subscriber with <see cref="MessageInstance"/> as an argument and return value.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    /// <param name="argument">Argument.</param>
    /// <param name="messageInstance">The instance information of this executing.</param>
    /// <returns>Return value.</returns>
    public delegate TReturn SubscriberWithMessageInstance<TParameter, TReturn>(TParameter argument, MessageInstance messageInstance);


    /// <summary>
    /// Async delegate of a subscriber without return value.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <param name="argument">Argument.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword='none'/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public delegate Task SubscriberAsync<TParameter>(TParameter argument, CancellationToken cancellationToken = default);

    /// <summary>
    /// Async delegate of a subscriber with return value.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    /// <param name="argument">Argument.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword='none'/>.</param>
    /// <returns>A task that represents the asynchronous operation, which wraps the return value.</returns>
    public delegate Task<TReturn> SubscriberAsync<TParameter, TReturn>(TParameter argument, CancellationToken cancellationToken = default);

    /// <summary>
    /// Async delegate of a subscriber with <see cref="MessageInstance"/> as an argument without return value.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <param name="argument">Argument.</param>
    /// <param name="messageInstance">The instance information of this executing.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword='none'/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public delegate Task SubscriberWithMessageInstanceAsync<TParameter>(TParameter argument, MessageInstance messageInstance, CancellationToken cancellationToken = default);

    /// <summary>
    /// Async delegate of a subscriber with <see cref="MessageInstance"/> as an argument and return value.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    /// <param name="argument">Argument.</param>
    /// <param name="messageInstance">The instance information of this executing.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword='none'/>.</param>
    /// <returns>A task that represents the asynchronous operation, which wraps the return value.</returns>
    public delegate Task<TReturn> SubscriberWithMessageInstanceAsync<TParameter, TReturn>(TParameter argument, MessageInstance messageInstance, CancellationToken cancellationToken = default);
}
