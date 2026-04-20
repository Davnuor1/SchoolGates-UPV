# Use an official Node.js runtime as a parent image
FROM node:14-slim

# Set the working directory in the container
WORKDIR /usr/src/app

# Copy the current directory contents into the container at /usr/src/app
COPY . .

# Install http-server in the container
RUN npm install -g http-server

# Make port 8080 available to the world outside this container
EXPOSE 8080

# Run http-server when the container launches
CMD ["http-server", ".", "-p", "8080", "-c-1"]
