#include <iostream>
#include <fstream>

#include "Shader.h"

static std::string Shader2SourceCodeStr(const std::string& fileName);
static GLboolean ShaderCompiles(GLuint shader);
static GLuint CreateShader(GLenum type, const std::string& text);

Shader::Shader()
{
	m_program = glCreateProgram();
}

Shader::~Shader()
{
	if (m_program == 0)
		return;

	for (unsigned int i = 0; i < NUM_SHADERS; i++)
	{
		glDetachShader(m_program, m_shaders[i]);
		glDeleteShader(m_shaders[i]);
	}
	glDeleteProgram(m_program);
}

/*
 * NAME:
 *	SetShader
 * DESCRIPTION:
 *	Creates shader of given type from file and attach to shader program
 * PARAMETERS:
 *	GLenum type: type of the shader, e.g. GL_VERTEX_SHADER
 *	const std::string& fileName: GLSL file
 * RETURN:
 *	GL_TRUE if shader is created and attached successfully; otherwise
 *	GL_FALSE
 */
GLboolean Shader::SetShader(GLenum type, const std::string& fileName)
{
	if (m_program == 0)
		return GL_FALSE;

	unsigned int idx;
	switch (type)
	{
	case GL_VERTEX_SHADER:
		idx = IDX_VERT_SHADER;
		break;
	case GL_FRAGMENT_SHADER:
		idx = IDX_FRAG_SHADER;
		break;
	default:
		idx = -1;
		break;
	}

	/* Create shader */
	if (0 <= idx && idx < NUM_SHADERS)
	{
		m_shaders[idx] = CreateShader(type, Shader2SourceCodeStr(fileName));
		
		if (m_shaders[idx] == 0)
			return GL_FALSE;

		/* Attach shader to program */
		glAttachShader(m_program, m_shaders[idx]);

		return GL_TRUE;
	}
	return GL_FALSE;
}

/*
 * NAME:
 *	RemoveShader
 * DESCRIPTION:
 *	Removes shader of given type from program
 * PARAMETERS:
 *	GLenum type: type of the shader, e.g. GL_VERTEX_SHADER
 * RETURN:
 *	GL_TRUE if shader is detached & deleted successfully; otherwise
 *	GL_FALSE
 */
GLboolean Shader::RemoveShader(GLenum type)
{
	if (m_program == 0)
		return GL_FALSE;

	unsigned int idx;
	switch (type)
	{
	case GL_VERTEX_SHADER:
		idx = IDX_VERT_SHADER;
		break;
	case GL_FRAGMENT_SHADER:
		idx = IDX_FRAG_SHADER;
		break;
	default:
		idx = -1;
		break;
	}

	if (idx >= 0)
	{
		glDetachShader(m_program, m_shaders[idx]);
		glDeleteShader(m_shaders[idx]);
		return GL_TRUE;
	}

	return GL_FALSE;
}

/*
 * NAME:
 *	LinkProgram
 * DESCRIPTION:
 *	Links a program object
 * PARAMETERS:
 *	NONE
 * RETURN:
 *	GL_TRUE if program is linked successfully; otherwise
 *	GL_FALSE
 */
GLboolean Shader::LinkProgram()
{
	if (m_program == 0)
		return GL_FALSE;

	GLint program_linked;
	
	glLinkProgram(m_program);
	glGetProgramiv(m_program, GL_LINK_STATUS, &program_linked);
	
	if (program_linked != GL_TRUE)
		return GL_FALSE;
	
	return GL_TRUE;
}

/*
 * NAME:
 *	ValidateProgram
 * DESCRIPTION:
 *	Validates a program object
 * PARAMETERS:
 *	NONE
 * RETURN:
 *	GL_TRUE if program is validated successfully; otherwise
 *	GL_FALSE
 */
GLboolean Shader::ValidateProgram()
{
	if (m_program == 0)
		return GL_FALSE;

	GLint program_validated;

	glValidateProgram(m_program);
	glGetProgramiv(m_program, GL_VALIDATE_STATUS, &program_validated);

	if (program_validated != GL_TRUE)
		return GL_FALSE;

	return GL_TRUE;
}

/*
 * NAME:
 *	Bind
 * DESCRIPTION:
 *	Installs a program object as part of current rendering state
 * PARAMETERS:
 *	NONE
 * RETURN:
 *	NONE
 */
void Shader::Bind()
{
	glUseProgram(m_program);
}

/***************************** HELPER BEGINS *******************************/
/*
 * NAME:
 *	Shader2SourceCodeStr
 * DESCRIPTION:
 *	Reads shader file and convert it to a string
 * PARAMETERS:
 *	const std::string& fileName: file name to be read in
 * RETURN:
 *	source code string
 */
static std::string Shader2SourceCodeStr(const std::string& fileName)
{
	std::ifstream file(fileName);
	std::string line;
	std::string output;

	if (file.is_open())
	{
		while (file.good())
		{
			getline(file, line);
			output.append(line + "\n");
		}
	}
	else
		std::cerr << "Error: fail to open shader file " << fileName << std::endl;

	return output;
}

/*
 * NAME:
 *	ShaderCompiles
 * DESCRIPTION:
 *	Checks if last compile operation on shader was successful
 * PARAMETERS:
 *	GLuint shader: shader to be checked
 * RETURN:
 *	GL_TRUE if last compile operation was successfully; otherwise
 *	GL_FALSE
 */
static GLboolean ShaderCompiles(GLuint shader)
{
	GLint status = GL_FALSE;

	glGetShaderiv(shader, GL_COMPILE_STATUS, &status);

	return status;
}

/*
 * NAME:
 *	CreateShader
 * DESCRIPTION:
 *	Creates shader object from given source code string
 * PARAMETERS:
 *	GLenum type: shader type
 *	const std::string& text: source code string
 * RETURN:
 *	0 if an error occurs creating the shader object; otherwise
 *	non-zero value by which it can be referenced
 */
static GLuint CreateShader(GLenum type, const std::string& text)
{
	GLuint shader = glCreateShader(type);

	if (shader == 0)
		std::cerr << "Error: fail to create shader object, type " << type << std::endl;

	// OpenGL is a C API, hence we have to convert our source
	// code string into C string
	// OpenGL allows multiple srouce code strings for a shader
	const GLchar* shaderSourceCodeStrings[1];
	GLint shaderSourceCodeStringLengths[1];
	shaderSourceCodeStrings[0] = text.c_str();
	shaderSourceCodeStringLengths[0] = text.length();

	glShaderSource(shader, 1, shaderSourceCodeStrings, shaderSourceCodeStringLengths);
	glCompileShader(shader);

	if (!ShaderCompiles(shader))
	{
		std::cerr << "Error: fail to compile shader " << shader << std::endl;
		std::cerr << text << std::endl;
		return 0;
	}

	return shader;
}
/***************************** HELPER ENDS *******************************/
