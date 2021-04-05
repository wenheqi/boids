#pragma once
#ifndef SHADER_H
#define SHADER_H

#include <string>
#include <GL/glew.h>

class Shader
{
public:
	Shader();
	virtual ~Shader();
	void Bind();
	GLboolean SetShader(GLenum type, const std::string& fileName);
	GLboolean RemoveShader(GLenum type);
	GLboolean LinkProgram();
	GLboolean ValidateProgram();
protected:
private:
	static const unsigned int NUM_SHADERS = 2;
	static const unsigned int IDX_VERT_SHADER = 0;
	static const unsigned int IDX_FRAG_SHADER = 1;
	GLuint m_program;
	GLuint m_shaders[NUM_SHADERS];
	Shader(const Shader& other) {}
	void operator=(const Shader& other) {}
};

#endif // SHADER_H
