

\# Asynchronous Booking Flow



\## Muc tieu

Xu ly dat ve bat dong bo bang RabbitMQ va Booking Worker.



\## Kien truc

Client -> YARP Gateway -> EventBooking.API

&#x20;                           |

&#x20;                        RabbitMQ

&#x20;                           |

&#x20;                    Booking Worker

&#x20;                           |

&#x20;                        SQL Server



\## Trang thai Booking

\- Pending: Dang cho xu ly.

\- Confirmed: Dat ve thanh cong.

\- Rejected: Dat ve bi tu choi.



\## Quy trinh

1\. Client gui yeu cau dat ve.

2\. API tao Booking trang thai Pending.

3\. API luu message vao Outbox.

4\. Publisher gui message den RabbitMQ.

5\. Worker nhan message.

6\. Worker kiem tra ve va xu ly transaction.

7\. Worker cap nhat Confirmed hoac Rejected.

8\. Client tra cuu trang thai Booking.



\## Yeu cau

\- Chong ban vuot ve.

\- Chong xu ly trung message.

\- Ho tro retry.

\- Dam bao nhat quan du lieu.



\## Tien do

Hien tai API xu ly dat ve dong bo.

RabbitMQ va Booking Worker chua duoc trien khai.



