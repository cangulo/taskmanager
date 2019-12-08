using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.CQRS.TasksCQ.Queries;
using TaskManagerAPI.Exceptions.Helpers;
using TaskManagerAPI.Filters.Authentication;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Models.FE.TasksDtos;

namespace TaskManagerAPI.Controllers
{
    [Route("api/tasks")]
    [AuthenticationFilter]
    public class TasksController : Controller
    {
        private readonly IErrorResponseCreator _errorResponseCreator;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TasksController(
            IErrorResponseCreator createErrorResponse,
            IMediator mediator,
            IMapper mapper)
        {
            _errorResponseCreator = createErrorResponse;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the task of the current user
        /// </summary>
        /// <returns></returns>
        /// <response code="200">List of Task attached</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            Result<IReadOnlyCollection<TaskDomain>> result = await _mediator.Send(new TaskCollectionQuery());
            var taskDtos = _mapper.Map<IEnumerable<TaskToGetDto>>(result.Value);
            return Ok(taskDtos);
        }

        /// <summary>
        /// Get a task by its Id
        /// </summary>
        /// <param name="id">Task Id</param>
        /// <returns></returns>
        /// <response code="200">List of Task attached</response>
        /// <response code="404">Task not found</response>
        [HttpGet("{id}", Name = "GetTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            Result<TaskDomain> result = await _mediator.Send(new TaskQuery { Id = id });
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<TaskToGetDto>(result.Value));
            }
            else
            {
                return _errorResponseCreator.CreateResponse(result.Errors);
            }
        }

        /// <summary>
        /// Create a task using an specific dto class as input
        /// </summary>
        /// <param name="taskDto">Task to create in dto model</param>
        /// <returns></returns>
        /// <response code="201">Task Created correctly</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTask([FromBody]TaskToBeCreatedDto taskDto)
        {
            TaskDomain taskToBeCreated = _mapper.Map<TaskDomain>(taskDto);

            Result opResult = await _mediator.Send(
                new CreateTaskCommand
                {
                    Task = taskToBeCreated
                });

            if (opResult.IsSuccess)
            {
                var taskCreated = _mapper.Map<TaskToGetDto>(taskToBeCreated);
                return CreatedAtRoute("GetTask", new { id = taskCreated.Id }, taskCreated);
            }
            else
            {
                return _errorResponseCreator.CreateResponse(opResult.Errors);
            }
        }

        /// <summary>
        /// Update all the fields of one task
        /// </summary>
        /// <param name="id">Task id</param>
        /// <param name="taskToBeUpdatedDto">Full new Task in DTO model</param>
        /// <returns></returns>
        /// <response code="204">Task Modified correctly</response>
        /// <response code="404">Task not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody]TaskForFullUpdatedDto taskToBeUpdatedDto)
        {
            var taskToBeFullUpdated = _mapper.Map<TaskForUpdated>(taskToBeUpdatedDto);
            Result opResult = await _mediator.Send(new UpdateTaskCommand
            {
                Id = id,
                Task = taskToBeFullUpdated
            });
            if (opResult.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                return _errorResponseCreator.CreateResponse(opResult.Errors);

            }
        }

        /// <summary>
        /// Update the fields specified in the request of one task specified by its id. 
        /// The request must follow JsonPatchDocument model to specify what fields must be modified.
        /// </summary>
        /// <param name="id">Task Id</param>
        /// <param name="patchDocTask">JsonPatchDocument with modifications to be applied</param>
        /// <returns></returns>
        /// <response code="204">Task Modified correctly</response>
        /// <response code="404">Task not found</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [FromBody]JsonPatchDocument<TaskForFullUpdatedDto> patchDocTask)
        {
            Result<TaskDomain> taskResult = await _mediator.Send(new TaskQuery { Id = id });
            if (taskResult.IsSuccess)
            {
                var taskForPartialUpdateDto = _mapper.Map<TaskForFullUpdatedDto>(taskResult.Value);
                patchDocTask.ApplyTo(taskForPartialUpdateDto);
                var taskForUpdate = _mapper.Map<TaskForUpdated>(taskForPartialUpdateDto);

                Result opResult = await _mediator.Send(new UpdateTaskCommand
                {
                    Id = id,
                    Task = taskForUpdate
                });
                if (opResult.IsSuccess)
                {
                    return NoContent();
                }
                else
                {
                    return _errorResponseCreator.CreateResponse(opResult.Errors);

                }
            }
            else
            {
                return _errorResponseCreator.CreateResponse(taskResult.Errors);
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="id">Task Id</param>
        /// <returns></returns>
        /// <response code="200">Task Deleted correctly</response>
        /// /// <response code="404">Task not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            Result opResult = await _mediator.Send(new DeleteTaskCommand
            {
                Id = id
            });
            if (opResult.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return _errorResponseCreator.CreateResponse(opResult.Errors);
            }
        }
    }
}
