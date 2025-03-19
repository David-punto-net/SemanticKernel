# Semantic Kernel  

Este repositorio contiene ejemplos sobre cómo utilizar **Semantic Kernel** para implementar **IA local**, conectando un **LLM** a través de **Ollama**.  

**Semantic Kernel** es una biblioteca de código abierto desarrollada por Microsoft que facilita la creación de aplicaciones de inteligencia artificial (IA) al integrar modelos de lenguaje (LLM) y herramientas de IA con aplicaciones personalizadas. 
Está diseñada para trabajar con modelos como GPT y otros similares, proporcionando una infraestructura para desarrollar y gestionar tareas complejas de procesamiento de lenguaje natural (NLP).

Algunas características clave de Semantic Kernel son:

**- Integración de LLMs**: Permite usar modelos de lenguaje como GPT-3, GPT-4 y otros, de manera sencilla y flexible, integrándolos directamente en las aplicaciones.

**- Orquestación de habilidades**: Ayuda a gestionar y orquestar diferentes "habilidades" (funciones o acciones) que la IA puede realizar, facilitando la creación de aplicaciones más avanzadas y personalizadas.

**- Interacción con herramientas externas**: Facilita la integración con servicios y herramientas externas como bases de datos, APIs, y sistemas de mensajería, lo que permite ampliar la funcionalidad de las aplicaciones.

**- Desarrollo de flujos de trabajo**: Los desarrolladores pueden definir y controlar el flujo de trabajo, asegurando que la IA ejecute tareas en el orden y con las condiciones adecuadas.

**- Fácil personalización**: Permite la creación de aplicaciones específicas sin necesidad de conocimientos avanzados en IA, enfocándose en hacer que la integración de IA sea accesible a desarrolladores de diferentes niveles.


## Tecnologías Utilizadas  
- **Semantic Kernel**: SDK de Microsoft para la integración de modelos de IA en aplicaciones .NET.  
- **Ollama**: Plataforma para ejecutar modelos de lenguaje de manera local.  
- **Llama 3.1**: Modelo de lenguaje (LLM) compatible con Ollama.  

## Funcionalidades  
**- Chat con historial**: Almacena y recupera conversaciones pasadas.  
**- Conexión con una LLM local** usando **Ollama** para procesar respuestas sin depender de la nube.  
**- Gestión de contexto** con **Semantic Kernel** para mejorar la coherencia en las respuestas.  

## Ejemplos Incluidos  
- **SmKChat:** Ejemplo de como conectar el LLM local llama3.1 atraves de Ollama y habilitar el un Chat con historial y asignarle una personalidad a la IA atraves del mensaje del sistema. 
- **Implementación de un chat con historial**  
- **Uso de Plugins en Semantic Kernel**  

## Requisitos  
- **.NET 8 SDK**  
- **Modelo de lenguaje compatible con Ollama**  
