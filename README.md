# Event Ticket Booking System

Đồ án: Hệ thống đặt vé sự kiện phân tán

## Công nghệ
- ASP.NET Core Web API (.NET 9)
- SQL Server / Entity Framework Core
- YARP API Gateway
- Swagger / OpenAPI

## Chức năng đã triển khai
- CRUD sự kiện
- Quản lý loại vé
- Đặt vé và quản lý số lượng vé
- Chống bán vượt vé bằng transaction
- Idempotency chống đặt vé trùng
- Cân bằng tải Round Robin
- Health Check, Failover, Auto Recovery

## Chức năng đang phát triển
- RabbitMQ
- Booking Worker
- Xử lý đặt vé bất đồng bộ
- Trạng thái Pending / Confirmed / Rejected
- Tra cứu đơn theo người dùng

## Cấu trúc dự án
- EventBooking.API
- EventBooking.Gateway

