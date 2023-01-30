using System.Net;
using Desafio.Domain;
using Microsoft.AspNetCore.Mvc;
using Desafio.Infrastructure;

namespace Desafio.Application;

public abstract class AbstractController : ControllerBase
{
    private readonly ILogger logger;

    private readonly IServiceProvider provider;

    protected AbstractController(IServiceProvider provider)
    {
        this.provider = provider;
        this.logger = provider.GetService<ILogger>()!;
    }

    public Task<IActionResult> SendAsync<R>(R request) where R : class, IRequest => SendAsync(request, false);

    public async Task<IActionResult> SendAsync<R>(R request, bool create) where R : class, IRequest
    {
        try
        {
            var status = create ? HttpStatusCode.Created : HttpStatusCode.OK;
            var response = await EventBus.SendAsync(provider, request);

            return Response(status, response);
        }
        catch (UnauthorizedException ex)
        {
            return Response(HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (NotFoundException ex)
        {
            return Response(HttpStatusCode.NotFound, ex.Message);
        }
        catch (ForbidenException ex)
        {
            return Response(HttpStatusCode.Forbidden, ex.Message);
        }
        catch (InvalidException ex)
        {
            return Response(HttpStatusCode.BadRequest, ex.Invalids);
        }
        catch (RequiredException ex)
        {
            return Response(HttpStatusCode.BadRequest, ex.Message);
        }
        catch (DomainException ex)
        {
            return Response(HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return Response(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public ObjectResult Response(HttpStatusCode status, object? value) => StatusCode((int)status, value);
}
