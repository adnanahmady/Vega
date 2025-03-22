define default
$(if $(1),$(1),$(2))
endef

define prefix
$(if $(1),$(2)$(1),)
endef

setup:
	mkdir -p .backend/home/.nuget
	mkdir -p .backend/home/.aspnet
	${MAKE} up
	${MAKE} shell run="sudo chmod -R 777 /home/docker/.nuget"
	${MAKE} shell run="sudo chmod -R 777 /home/docker/.aspnet"
	${MAKE} shell run="dotnet restore"
	${MAKE} shell run="dotnet dev-certs https --trust"

up:
	@docker compose up -d ${up-with}

build:
	${MAKE} up-with=--build

down:
	@docker compose down ${down-with}

restart:
	${MAKE} down up

shell:
	@docker compose exec $(call default,${service},backend) $(call default,${run},bash)

ps:
	@docker compose ps

volumes:
	@docker compose ps
	@echo "\nVolumes and their containers:"
	@for container in $$(docker compose ps -q); do \
		echo "\nContainer: $$(docker container inspect --format '{{.Name}}' $$container)"; \
		docker container inspect -f '{{range .Mounts}}{{printf "Volume: %s\nTarget: %s\nSource: %s\nType: %s\n" .Name .Destination .Source .Type}}{{end}}' $$container; \
	done

status: ps volumes

logs:
	@docker compose logs $(call default,${service},backend) ${with}

run: start
start:
	${MAKE} shell run="dotnet run --project Vega"

restore:
	${MAKE} shell run="dotnet restore"

test:
	${MAKE} shell run="dotnet test $(call default,$(call prefix,${filter},--filter=),$(call prefix,${f},--filter=)) ${with}"

fix:
	${MAKE} shell run="dotnet format style"
	${MAKE} shell run="dotnet format style --severity=info"
	${MAKE} shell run="dotnet format whitespace"
