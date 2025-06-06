using LiveMap.Application.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace LiveMapDashboard.Web.Extensions.Controllers;

public static class ControllerExtensions
{
    #region Response message generic

    public static void BuildResponseMessage<T>(
        this Controller controller,
        ExternalHttpResponse<T> result,
        Dictionary<string, string>? messages = null)
    {
        controller.BuildResponseMessage(
            result: result,
            dataStore: controller.ViewData,
            messages: messages);
    }
    public static void BuildResponseMessageForRedirect<T>(
        this Controller controller,
        ExternalHttpResponse<T> result,
        Dictionary<string, string>? messages = null)
    {
        controller.BuildResponseMessage(
            result: result,
            dataStore: controller.TempData,
            messages: messages);
    }
    public static void BuildResponseMessage<T>(
        this Controller controller, 
        ExternalHttpResponse<T> result,
        IDictionary<string, object?> dataStore,
        Dictionary<string, string>? messages = null)
    {
        if(messages is null)
        {
            messages = controller.CreateMessageDictionary();
        }

        if (result.IsSuccess)
        {
            dataStore["SuccessMessage"] = messages["SuccessMessage"];
        }
        else
        {
            dataStore["ErrorMessage"] = result.StatusCode switch
            {
                HttpStatusCode.BadRequest => messages["ErrorMessage"],
                HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                _ => "Something went wrong while trying to contact the application. Please try again later"
            };
        }
    }

    #endregion Response message generic

    #region Response message non-generic

    public static void BuildResponseMessage(
        this Controller controller,
        ExternalHttpResponse result,
        Dictionary<string, string>? messages = null)
    {
        controller.BuildResponseMessage(
            result: result,
            dataStore: controller.ViewData,
            messages: messages);
    }
    
    public static void BuildResponseMessageForRedirect(
        this Controller controller,
        ExternalHttpResponse result,
        Dictionary<string, string>? messages = null)
    {
        controller.BuildResponseMessage(
            result: result,
            dataStore: controller.TempData,
            messages: messages);
    }
   
    public static void BuildResponseMessage(
        this Controller controller,
        ExternalHttpResponse result,
        IDictionary<string, object?> dataStore,
        Dictionary<string, string>? messages = null)
    {
        if (messages is null)
        {
            messages = controller.CreateMessageDictionary();
        }

        if (result.IsSuccess)
        {
            dataStore["SuccessMessage"] = "Your request was successfully processed!";
        }
        else
        {
            dataStore["ErrorMessage"] = result.StatusCode switch
            {
                HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
                HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                _ => "Something went wrong while trying to contact the application. Please try again later"
            };
        }
    }
    
    #endregion Response message non-generic
    
    public static Dictionary<string, string> CreateMessageDictionary(
        this Controller controller,
        string success = "Your request was successfully processed!", 
        string error = "The submitted data was invalid. Please check the data you submitted.") 
    {
        return new Dictionary<string, string>
        {
            { "SuccessMessage", success },
            { "ErrorMessage", error }
        };
    }
}
