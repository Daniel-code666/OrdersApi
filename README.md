# Orders

Sistema de gestión de órdenes evolucionado hacia una arquitectura distribuida basada en microservicios, incorporando un API Gateway, mensajería por eventos y procesamiento asíncrono.

## Arquitectura

La solución está compuesta por los siguientes proyectos:

- `Orders`: API principal para la gestión de órdenes
- `Orders.Gateway`: API Gateway implementado con YARP
- `Orders.Worker`: Worker Service para el consumo de eventos
- `Orders.Application`: lógica de aplicación e interfaces
- `Order.Domain`: entidades y reglas del dominio
- `Order.Infrastructure`: persistencia e integraciones externas

## Flujo implementado

Cliente → Orders.Gateway → Orders API → RabbitMQ → Orders.Worker → Mailpit

## Funcionalidades implementadas

- Creación de órdenes
- Consulta de órdenes por id
- Consulta de órdenes con filtros
- Enrutamiento mediante API Gateway con YARP
- Publicación del evento `OrderCreated` al crear una orden
- Consumo del evento mediante un Worker Service
- Envío de correo de prueba usando Mailpit tras consumir el evento

## Servicios disponibles
- `Orders API`: http://localhost:8080
- `Orders Gateway`: http://localhost:8081
- `RabbitMQ Management`: http://localhost:15672
- `Mailpit`: http://localhost:8025

## Infraestructura

La solución se ejecuta con Docker Compose e incluye:

- PostgreSQL
- RabbitMQ
- Mailpit
- Orders API
- Orders.Gateway
- Orders.Worker

## Ejecución

Desde la raíz del proyecto:

```bash
docker compose up --build
