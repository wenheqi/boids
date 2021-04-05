#include <iostream>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
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

	/* Loop until the user closes the window */
	while (!glfwWindowShouldClose(window))
	{
		/* Render here */
		glClear(GL_COLOR_BUFFER_BIT);

		/* Swap front and back buffers */
		glfwSwapBuffers(window);

		/* Poll for and process events */
		glfwPollEvents();
	}

	glfwTerminate();
	return 0;
}
