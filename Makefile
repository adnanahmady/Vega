define default
$(if $(1),$(1),$(2))
endef

define prefix
$(if $(1),$(2)$(1),)
endef

up:
	@docker compose up -d ${up-with}

down:
	@docker compose down ${down-with}

restart:
	${MAKE} down up

shell:
	@docker compose exec $(call default,${service},database) bash

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
	@docker compose logs $(call default,${service},database) ${with}

run: start
start:
	@dotnet run --project Vega

test:
	@dotnet test $(call default,$(call prefix,${filter},--filter=),$(call prefix,${f},--filter=)) ${with}

fix:
	@dotnet format style
	@dotnet format style --severity=info
	@dotnet format whitespace
