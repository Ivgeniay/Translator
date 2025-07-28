# Real-time Audio Translator

Приложение для синхронного перевода аудио в реальном времени с использованием AI технологий.

## Архитектура

```
Unity Client → SignalR → ASP.NET Core API → AI Service (LangChain + TypeScript) → OpenAI APIs
     ↑                                                                                    ↓
     └── Переведенное аудио ←← Обработанный результат ←← [STT → Translation → TTS] ←←─────┘
```

## Структура проекта

```
root/
├── .github/workflows/
│   └── deploy.yml              # CI/CD пайплайн
├── .vscode/                    # VS Code workspace настройки
├── ai-translate-service/       # AI сервис (LangChain + TypeScript)
├── unity-client/              # Unity 6+ клиент
├── backend/                   # ASP.NET Core 9 + SignalR API
├── commonLib/                 # .NET Standard 2.1 общая библиотека
└── tools/                     # Скрипты автогенерации TypeScript моделей
```

## Компоненты

### Unity Client

-   Захват и обработка аудио
-   Настройки пользователя (голос, качество, языки)
-   Real-time коммуникация через SignalR
-   Воспроизведение переведенного аудио

### Backend (ASP.NET Core 9)

-   SignalR Hub для real-time коммуникации
-   Проксирование запросов к AI сервису
-   Валидация и логирование
-   Управление сессиями пользователей

### AI Translate Service (LangChain + TypeScript)

-   **Speech-to-Text**: OpenAI Whisper API
-   **Translation**: OpenAI GPT модели
-   **Text-to-Speech**: OpenAI TTS API
-   Настраиваемые параметры обработки

### CommonLib (.NET Standard 2.1)

-   Общие модели данных (DTOs)
-   Конфигурационные классы
-   Enums и константы
-   Валидация атрибуты

## Технологии

| Компонент    | Технологии                                 |
| ------------ | ------------------------------------------ |
| Unity Client | Unity 6+, C#, SignalR Client               |
| Backend      | ASP.NET Core 9, SignalR, C#                |
| AI Service   | Node.js, TypeScript, LangChain, OpenAI API |
| CommonLib    | .NET Standard 2.1, C#                      |
| DevOps       | GitHub Actions, Docker                     |

## Возможности

### Настройки на клиенте:

-   Выбор голоса и акцента для TTS
-   Длительность аудио сегментов
-   Качество аудио (sample rate, bitrate)
-   Языки источника и перевода
-   Включение текстового ответа для логов

### Особенности:

-   Real-time обработка через SignalR
-   Автогенерация TypeScript моделей из .NET
-   Гибкие пользовательские настройки
-   Детальное логирование процесса
-   Безопасная передача данных

## Быстрый старт

### Требования

-   .NET 9 SDK
-   Node.js 18+
-   Unity 6+
-   OpenAI API Key

## Разработка

## CI/CD

Автоматическое развертывание настроено через GitHub Actions:

-   ✅ Тестирование всех компонентов
-   📦 Сборка Docker образов
-   🚀 Развертывание в облако
-   🔄 Автогенерация TS моделей
