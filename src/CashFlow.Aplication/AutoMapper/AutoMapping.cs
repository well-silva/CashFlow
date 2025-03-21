using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using ClashFlow.Domain.Entities;

namespace CashFlow.Aplication.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }

        private void RequestToEntity()
        {
            CreateMap<RequestRegisterExpenseDto, Expense>();
        }

        private void EntityToResponse()
        {
            CreateMap<Expense, ResponseRegisteredExpense>();
            CreateMap<Expense, ResponseShortExpense>();
            CreateMap<Expense, ResponseExpense>();
        }
    }
}
