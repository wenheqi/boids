#include <iostream>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <glm/glm.hpp>
#include "Shader/Shader.h"

// The program by default runs in the project folder, hence we should use the relative path
// wrt the project folder if needed.
// https://stackoverflow.com/questions/32643393/how-to-open-input-file-c-on-visual-studio-community-2015
// under Windows, you have to have an extra slash to escape the slash in path
#define vert_shader_file "res\\shaders\\basicShader.vs"
#define frag_shader_file "res\\shaders\\basicShader.fs"

int main(void)
{
	GLFWwindow* window;

	/* Initialize the library */
	if (!glfwInit())
		return -1;

	/* Create a windowed mode window and its OpenGL context */
	window = glfwCreateWindow(640, 480, "Hello World", NULL, NULL);
	if (!window)
	{
		glfwTerminate();
		return -1;
	}

	/* Make the window's context current */
	glfwMakeContextCurrent(window);

	/* Initialize GLEW */
	if (glewInit() != GLEW_OK)
	{
		std::cout << "ERROR: fail to initialize GLEW" << std::endl;
		glfwTerminate();
		return -1;
	}

	/* Print OpenGL version */
	std::cout << "OpenGL Version: " << glGetString(GL_VERSION) << std::endl;

	/* Create shader */
	Shader shader;
	if (!shader.SetShader(GL_VERTEX_SHADER, vert_shader_file))
	{
		std::cerr << "Error: fail to create vertex shader" << std::endl;
		return -1;
	}
	if (!shader.SetShader(GL_FRAGMENT_SHADER, frag_shader_file))
	{
		std::cerr << "Error: fail to create fragment shader" << std::endl;
		return -1;
	}
	if (!shader.LinkProgram())
	{
		std::cerr << "ERROR: fail to link shader program" << std::endl;
		return -1;
	}
	if (!shader.ValidateProgram())
	{
		std::cerr << "ERROR: fail to validate shader program" << std::endl;
		return -1;
	}

	/* Create VAO (Vertex Array Object) */
	GLuint VertexArrayID;
	glGenVertexArrays(1, &VertexArrayID);
	glBindVertexArray(VertexArrayID);

	// An array of 3 vectors which represents 3 vertices
	static const GLfloat g_vertex_buffer_data[] = {
		-1.0f, -1.0f, 0.0f,
		 1.0f, -1.0f, 0.0f,
		 0.0f,  1.0f, 0.0f,
	};

	// This will identify our vertex buffer
	GLuint vertexbuffer;
	// Generate 1 buffer, put the resulting identifier in vertexbuffer
	glGenBuffers(1, &vertexbuffer);
	// The following commands will talk about our 'vertexbuffer' buffer
	glBindBuffer(GL_ARRAY_BUFFER, vertexbuffer);
	// Give our vertices to OpenGL.
	glBufferData(GL_ARRAY_BUFFER, sizeof(g_vertex_buffer_data), g_vertex_buffer_data, GL_STATIC_DRAW);

	/* Loop until the user closes the window */
	while (!glfwWindowShouldClose(window))
	{
		/* Render here */
		glClear(GL_COLOR_BUFFER_BIT);

		shader.Bind();
		// 1st attribute buffer : vertices
		glEnableVertexAttribArray(0);
		glBindBuffer(GL_ARRAY_BUFFER, vertexbuffer);
		glVertexAttribPointer(
			0,                  // attribute 0. No particular reason for 0, but must match the layout in the shader.
			3,                  // size
			GL_FLOAT,           // type
			GL_FALSE,           // normalized?
			0,                  // stride
			(void*)0            // array buffer offset
		);
		// Draw the triangle !
		glDrawArrays(GL_TRIANGLES, 0, 3); // Starting from vertex 0; 3 vertices total -> 1 triangle
		glDisableVertexAttribArray(0);

		/* Swap front and back buffers */
		glfwSwapBuffers(window);

		/* Poll for and process events */
		glfwPollEvents();
	}

	glfwTerminate();
	return 0;
}
