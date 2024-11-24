job "aurora" {
  type = "service"

  group "aurora" {
    network {
      port "http" {
        to = 3000
      }
    }

    service {
      name     = "aurora"
      port     = "http"
      provider = "nomad"
      tags = [
        "traefik.enable=true",
        "traefik.http.routers.aurora.rule=Host(`aurora.datasektionen.se`)",
        "traefik.http.routers.aurora.tls.certresolver=default",
      ]
    }

    task "aurora" {
      driver = "docker"

      config {
        image = var.image_tag
        ports = ["http"]
      }

      resources {
        memory = 10
      }
    }
  }
}

variable "image_tag" {
  type = string
  default = "ghcr.io/datasektionen/aurora:latest"
}
