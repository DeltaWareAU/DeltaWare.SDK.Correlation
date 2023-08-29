using Microsoft.Extensions.DependencyInjection;
using System;
using TraceLink.Abstractions.Context;
using TraceLink.Abstractions.Forwarder;
using TraceLink.Abstractions.Providers;

// ReSharper disable once CheckNamespace
namespace TraceLink.AspNetCore.Options.Builder
{
    public static partial class CorrelationOptionsBuilderExtensions
    {
        /// <summary>
        /// Overrides the default <see cref="IIdProvider"/> with the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TIdProvider">The <see cref="IIdProvider"/> to be used when generating Correlation Ids.</typeparam>
        /// <remarks>Registers the <see cref="IIdProvider"/> as <see cref="IIdProvider{TContext}"/>.</remarks>
        public static void UseIdProvider<TIdProvider>(this ICorrelationOptionsBuilder builder) where TIdProvider : IIdProvider
        {
            builder.Services.AddSingleton<IIdProvider<CorrelationContext>, IdProviderWrapper<CorrelationContext, TIdProvider>>();
        }

        /// <summary>
        /// Overrides the default <see cref="IIdProvider"/> with the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TIdProvider">The <see cref="IIdProvider"/> to be used when generating Correlation Ids.</typeparam>
        /// <param name="instance">The instance to override the default <see cref="IIdProvider"/> with.</param>
        /// <remarks>Registers the <see cref="IIdProvider"/> as <see cref="IIdProvider{TContext}"/>.</remarks>
        public static void UseIdProvider<TIdProvider>(this ITraceOptionsBuilder builder, TIdProvider instance) where TIdProvider : IIdProvider
        {
            builder.Services.AddSingleton<IIdProvider<CorrelationContext>>(new IdProviderWrapper<CorrelationContext, TIdProvider>(instance));
        }

        /// <summary>
        /// Overrides the default <see cref="IIdProvider"/> with the provided instance.
        /// </summary>
        /// <typeparam name="TIdProvider">The <see cref="IIdProvider"/> to be used when generating Correlation Ids.</typeparam>
        /// <param name="instanceBuilder">The builds an <see cref="IIdProvider"/> to override the default <see cref="IIdProvider"/> with.</param>
        /// <remarks>Registers the <see cref="IIdProvider"/> as <see cref="IIdProvider{TContext}"/>.</remarks>
        public static void UseIdProvider<TIdProvider>(this ITraceOptionsBuilder builder, Func<IServiceProvider, TIdProvider> instanceBuilder) where TIdProvider : class, IIdProvider
        {
            builder.Services.AddSingleton<IIdProvider<CorrelationContext>>(p => new IdProviderWrapper<CorrelationContext, TIdProvider>(instanceBuilder.Invoke(p)));
        }

        /// <summary>
        /// Overrides the default <see cref="IIdForwarder"/> with the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TIdForwarder">The <see cref="IIdForwarder{TContet}"/> to be used when forwarding Correlation Ids.</typeparam>
        public static void UseIdForwarder<TIdForwarder>(this ITraceOptionsBuilder builder) where TIdForwarder : class, IIdForwarder<CorrelationContext>
        {
            builder.Services.AddSingleton<IIdForwarder<CorrelationContext>, TIdForwarder>();
        }

        /// <summary>
        /// Overrides the default <see cref="IIdForwarder{TContet}"/> with the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TIdForwarder">The <see cref="IIdForwarder{TContet}"/> to be used when forwarding Correlation Ids.</typeparam>
        /// <param name="instance">The instance to override the default <see cref="IIdForwarder{TContext}"/> with.</param>
        public static void UseIdForwarder<TIdForwarder>(this ITraceOptionsBuilder builder, TIdForwarder instance) where TIdForwarder : class, IIdForwarder<CorrelationContext>
        {
            builder.Services.AddSingleton<IIdForwarder<CorrelationContext>, TIdForwarder>();
        }

        /// <summary>
        /// Overrides the default <see cref="IIdForwarder{TContet}"/> with the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TIdForwarder">The <see cref="IIdForwarder{TContet}"/> to be used when forwarding Correlation Ids.</typeparam>
        /// <param name="instanceBuilder">The builds an <see cref="IIdForwarder{TContet}"/> to override the default <see cref="IIdProvider"/> with.</param>
        public static void UseIdForwarder<TIdForwarder>(this ITraceOptionsBuilder builder, Func<IServiceProvider, TIdForwarder> instanceBuilder) where TIdForwarder : class, IIdForwarder<CorrelationContext>
        {
            builder.Services.AddSingleton<IIdForwarder<CorrelationContext>>(instanceBuilder.Invoke);
        }
    }
}
