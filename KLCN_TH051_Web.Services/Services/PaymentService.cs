using KLCN_TH051_Web.Repositories.Data;
using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Enums;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Web.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            var course = await _context.Courses.FindAsync(request.CourseId);
            if (course == null)
                throw new Exception("Course not found");

            var payment = new Payment
            {
                StudentId = request.StudentId,
                CourseId = request.CourseId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                TransactionId = request.TransactionId,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return new PaymentResponse(payment);
        }

        public async Task<PaymentResponse> GetPaymentByIdAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                throw new Exception("Payment not found");

            return new PaymentResponse(payment);
        }

        public async Task<List<PaymentResponse>> GetPaymentsByStudentAsync(int studentId)
        {
            var payments = await _context.Payments
                .Where(p => p.StudentId == studentId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            return payments.Select(p => new PaymentResponse(p)).ToList();
        }

        public async Task<PaymentResponse> UpdatePaymentStatusAsync(int id, UpdatePaymentStatusRequest request)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                throw new Exception("Payment not found");

            payment.Status = request.Status;
            payment.TransactionId = request.TransactionId ?? payment.TransactionId;
            payment.LastUpdatedBy = "system"; // hoặc lấy từ token
            payment.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return new PaymentResponse(payment);
        }
    }
}
